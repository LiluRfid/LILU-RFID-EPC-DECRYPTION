// RFID decryption EPC
// Author: Jose A. Garcia-Uceda Calvo
// email: jose@lilu-enterprises.com
// web: www.lilu-enterprises.com


public class LiLuEPCc
{

    #region SGTIN96 Methods

                    // SCHEME :          SGTIN96
                    // URI TEMPLETE :    urn:epc:tag:
                    // TOTAL BITS:       96   
                    // NOTE:            
                    // we pass the partiton value and the mode(bits or digits) and return the company
                    // dimension in bits or digits and the same with the item
                    private static int SGTIN96_GetCompanyLenght(int partitionValue, CompanyPrefixLengthMode mode)
                    {
                        switch (mode)
                        {
                            case CompanyPrefixLengthMode.Bits:
                                switch (partitionValue)
                                {
                                    case 0:
                                        return 40;
                                    case 1:
                                        return 37;
                                    case 2:
                                        return 34;
                                    case 3:
                                        return 30;
                                    case 4:
                                        return 27;
                                    case 5:
                                        return 24;
                                    case 6:
                                        return 20;
                                    default:
                                        string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                                        throw new ApplicationException(errorMessage);
                                }
                            case CompanyPrefixLengthMode.Digits:
                                switch (partitionValue)
                                {
                                    case 0:
                                        return 12;
                                    case 1:
                                        return 11;
                                    case 2:
                                        return 10;
                                    case 3:
                                        return 9;
                                    case 4:
                                        return 8;
                                    case 5:
                                        return 7;
                                    case 6:
                                        return 6;
                                    default:
                                        string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                                        throw new ApplicationException(errorMessage);
                                }
                            default:
                                throw new ApplicationException(INV_EPC_LENGTHMODE);
                        }
                    }
                    private static int SGTIN96_GetItemLenght(int partitionValue, CompanyPrefixLengthMode mode)
                    {
                        switch (mode)
                        {
                            case CompanyPrefixLengthMode.Bits:
                                switch (partitionValue)
                                {
                                    case 0:
                                        return 4;
                                    case 1:
                                        return 7;
                                    case 2:
                                        return 10;
                                    case 3:
                                        return 14;
                                    case 4:
                                        return 17;
                                    case 5:
                                        return 20;
                                    case 6:
                                        return 24;
                                    default:
                                        string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                                        throw new ApplicationException(errorMessage);
                                }
                            case CompanyPrefixLengthMode.Digits:
                                switch (partitionValue)
                                {
                                    case 0:
                                        return 1;
                                    case 1:
                                        return 2;
                                    case 2:
                                        return 3;
                                    case 3:
                                        return 4;
                                    case 4:
                                        return 5;
                                    case 5:
                                        return 6;
                                    case 6:
                                        return 7;
                                    default:
                                        string errorMessage = string.Format(INV_EPC_PARTITION, partitionValue);
                                        throw new ApplicationException(errorMessage);
                                }
                            default:
                                throw new ApplicationException(INV_EPC_LENGTHMODE);
                        }
                    }
                    private static string SGTIN96_96_Decode(string binaryEpc, string header)
                    {
                        // Header
                        string SGTIN96_header = BinaryToDecimal(header).ToString();
                        // Filter value
                        string SGTIN96_filter = BinaryToDecimal(binaryEpc.Substring(8, 3)).ToString();
                        // Partition value
                        string SGTIN96_partition = BinaryToDecimal(binaryEpc.Substring(11, 3)).ToString();
                        // Company prefix
                        string SGTIN96_company = "";
                        // Item prefix
                        string SGTIN96_item = "";
                        // extension 41 bits. 96-38=58, empieza en el bit 58
                        string SGTIN96_extension = BinaryToDecimal(binaryEpc.Substring(58, 38)).ToString();
                        // Length of the company prefix and item in bits and digital
                        int SGTIN96_company_decimal = 0, SGTIN96_company_bits = 0;
                        int SGTIN96_item_decimal = 0, SGTIN96_item_bits = 0;

                        SGTIN96_company_bits = SGTIN96_GetCompanyLenght(Convert.ToInt32(SGTIN96_partition), CompanyPrefixLengthMode.Bits);
                        SGTIN96_item_bits = SGTIN96_GetItemLenght(Convert.ToInt32(SGTIN96_partition), CompanyPrefixLengthMode.Bits);

                        SGTIN96_company_decimal = SGTIN96_GetCompanyLenght(Convert.ToInt32(SGTIN96_partition), CompanyPrefixLengthMode.Digits);
                        SGTIN96_item_decimal = SGTIN96_GetItemLenght(Convert.ToInt32(SGTIN96_partition), CompanyPrefixLengthMode.Digits);

                        // lo que hace PadLeft es añadir ceros a la izquierda del String hasta completar la longitud que requiere
                        // SSCC_company_decimal o SSCC_company_item. Pq hacemos esto? pues pq cuando convertimos de binario a decimal
                        // si a la izquierda hay un cero lo perdemos. Entonces tenemos que completarlo con padleft
                        // en el caso que no halla ningun cero a la izquierda no hara nada.
                        SGTIN96_company = BinaryToDecimal(binaryEpc.Substring(14, SGTIN96_company_bits)).ToString().PadLeft(SGTIN96_company_decimal, '0');
                        SGTIN96_item = BinaryToDecimal(binaryEpc.Substring(14 + SGTIN96_company_bits, SGTIN96_item_bits)).ToString().PadLeft(SGTIN96_item_decimal, '0');


                        // sumamos todo para mostrar la trama entera separada por puntos para que los elementos sean mas visibles
                        // en esta codificacion incluimos la extension, lo dice el standard
                        string SGTIN96_trama = SGTIN96_header + "." + SGTIN96_filter + "." + SGTIN96_partition + "." + SGTIN96_company + "." + SGTIN96_item + "." + SGTIN96_extension;

                        return SGTIN96_trama;


                    }

                #endregion

}
