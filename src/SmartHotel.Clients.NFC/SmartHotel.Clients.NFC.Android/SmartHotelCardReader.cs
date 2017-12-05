using Android.Nfc;
using Android.Nfc.Tech;
using Android.Util;
using System;

namespace SmartHotel.Clients.NFC.Droid
{
    public class SmartHotelCardReader : Java.Lang.Object, NfcAdapter.IReaderCallback
    {
        private static readonly string TAG = "LoyaltyCardReader";

        // AID for our loyalty card service.
        private static readonly string SAMPLE_LOYALTY_CARD_AID = "F222222222";

        // ISO-DEP command HEADER for selecting an AID.
        // Format: [Class | Instruction | Parameter 1 | Parameter 2]
        private static readonly string SELECT_APDU_HEADER = "00A40400";

        // "OK" status word sent in response to SELECT AID command (0x9000)
        private static readonly byte[] SELECT_OK_SW = { (byte)0x90, (byte)0x00 };

        // Weak reference to prevent retain loop. _messageCallback is responsible for exiting
        // foreground mode before it becomes invalid (e.g. during onPause() or onStop()).
        private WeakReference<MessageCallback> _messageCallback;

        public interface MessageCallback
        {
            void OnMessageRecieved(string account);
        }

        public SmartHotelCardReader(WeakReference<MessageCallback> messageCallback)
        {
            _messageCallback = messageCallback;
        }

        /// <summary>
        /// Callback when a new tag is discovered by the system.
        /// Communication with the card should take place here.
        /// </summary>   
        /// <param name="tag">Discovered tag</param>
        public void OnTagDiscovered(Tag tag)
        {
            Log.Info(TAG, "New tag discovered");
            IsoDep isoDep = IsoDep.Get(tag);
            if (isoDep != null)
            {
                try
                {
                    // Connect to the remote NFC device
                    isoDep.Connect();

                    // Build SELECT AID command for our loyalty card service.
                    // This command tells the remote device which service we wish to communicate with.
                    Log.Info(TAG, "Requesting remote AID: " + SAMPLE_LOYALTY_CARD_AID);
                    byte[] command = BuildSelectApdu(SAMPLE_LOYALTY_CARD_AID);

                    // Send command to remote device
                    Log.Info(TAG, "Sending: " + ByteArrayToHexString(command));
                    byte[] result = isoDep.Transceive(command);

                    // If AID is successfully selected, 0x9000 is returned as the status word (last 2
                    // bytes of the result) by convention. Everything before the status word is
                    // optional payload, which is used here to hold the account number.
                    int resultLength = result.Length;
                    byte[] statusWord = { result[resultLength - 2], result[resultLength - 1] };
                    byte[] payload = new byte[resultLength - 2];
                    Array.Copy(result, payload, resultLength - 2);
                    bool arrayEquals = SELECT_OK_SW.Length == statusWord.Length;

                    for (int i = 0; i < SELECT_OK_SW.Length && i < statusWord.Length && arrayEquals; i++)
                    {
                        arrayEquals = (SELECT_OK_SW[i] == statusWord[i]);
                        if (!arrayEquals)
                            break;
                    }
                    if (arrayEquals)
                    {
                        // The remote NFC device will immediately respond with its stored account number
                        string accountNumber = System.Text.Encoding.UTF8.GetString(payload);
                        Log.Info(TAG, "Received: " + accountNumber);

                        // Inform CardReaderFragment of received account number
                        if (_messageCallback.TryGetTarget(out MessageCallback accountCallback))
                        {
                            accountCallback.OnMessageRecieved(accountNumber);
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(TAG, "Error communicating with card: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Build APDU for SELECT AID command. This command indicates which service a reader is
        /// interested in communicating with. See ISO 7816-4.
        /// </summary>   
        /// <param name="aid">Application ID (AID) to select</param>
        /// <returns>APDU for SELECT AID command</returns>
        public static byte[] BuildSelectApdu(string aid)
        {
            // Format: [CLASS | INSTRUCTION | PARAMETER 1 | PARAMETER 2 | LENGTH | DATA]
            return HexStringToByteArray(SELECT_APDU_HEADER + (aid.Length / 2).ToString("X2") + aid);
        }

        /// <summary>
        /// Utility class to convert a byte array to a hexadecimal string.
        /// </summary>   
        /// <param name="bytes">Bytes to convert</param>
        /// <returns>String, containing hexadecimal representation.</returns>
        public static string ByteArrayToHexString(byte[] bytes)
        {
            String s = string.Empty;

            for (int i = 0; i < bytes.Length; i++)
            {
                s += bytes[i].ToString("X2");
            }
            return s;
        }

        /// <summary>
        /// Utility class to convert a hexadecimal string to a byte string.
        /// </summary>   
        /// <param name="s">String containing hexadecimal characters to convert</param>
        /// <returns>Byte array generated from input</returns>
        private static byte[] HexStringToByteArray(string s)
        {
            int len = s.Length;

            if (len % 2 == 1)
            {
                throw new ArgumentException("Hex string must have even number of characters");
            }

            byte[] data = new byte[len / 2]; //Allocate 1 byte per 2 hex characters
            for (int i = 0; i < len; i += 2)
            {
                ushort val, val2;

                // Convert each chatacter into an unsigned integer (base-16)
                try
                {
                    val = (ushort)Convert.ToInt32(s[i].ToString() + "0", 16);
                    val2 = (ushort)Convert.ToInt32("0" + s[i + 1].ToString(), 16);
                }
                catch (Exception)
                {
                    continue;
                }

                data[i / 2] = (byte)(val + val2);
            }

            return data;
        }
    }
}