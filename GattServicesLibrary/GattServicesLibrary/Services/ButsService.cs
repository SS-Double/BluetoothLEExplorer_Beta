using System;
using System.Threading.Tasks;
using GattServicesLibrary.Helpers;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using System.ComponentModel;

namespace GattServicesLibrary.Services
{
    public class ButsService : GenericGattService
    {

        /// <summary>
        /// Name of the service
        /// </summary>
        public override string Name
        {
            get
            {
                return "Bluetooth Uart Service";
            }
        }

        private static Guid guid_Buts = new Guid();
        private static Guid guid_Buts_RxCharacteristic = new Guid();
        private static Guid guid_Buts_TxCharacteristic = new Guid();

        /// <summary>
        /// This characteristic is used to send a uart data.
        /// </summary>
        private GenericGattCharacteristic butsTx;

        /// <summary>
        /// Gets or Sets the buts tx characteristic
        /// </summary>
        public GenericGattCharacteristic ButsTx
        {
            get
            {
                return butsTx;
            }

            set
            {
                if (butsTx != value)
                {
                    butsTx = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ButsTx"));
                }
            }
        }

        /// <summary>
        /// This characteristic is used to receive a uart data.
        /// </summary>
        private GenericGattCharacteristic butsRx;

        /// <summary>
        /// Gets or Sets the buts tx characteristic
        /// </summary>
        public GenericGattCharacteristic ButsRx
        {
            get
            {
                return butsRx;
            }

            set
            {
                if (butsRx != value)
                {
                    butsRx = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("ButsRx"));
                }
            }
        }

        /// <summary>
        /// Starts the Heart rate service
        /// </summary>
        public override async Task Init()
        {
            await CreateServiceProvider(guid_Buts);

            // Preparing the Bluetooth Uart Tx characteristics
            var butsRxCharacteristics = PlainNotifyParameters;
            butsRxCharacteristics.UserDescription = "Bluetooth Uart Tx(Hex)";
            butsRxCharacteristics.PresentationFormats.Add(
                GattPresentationFormat.FromParts(
                    Convert.ToByte(PresentationFormats.FormatTypes.Unsigned16BitInteger),
                    PresentationFormats.Exponent,
                    Convert.ToUInt16(PresentationFormats.Units.Unitless),
                    Convert.ToByte(PresentationFormats.NamespaceId.BluetoothSigAssignedNumber),
                    PresentationFormats.Description));

            // Create the heart rate characteristic for the service
            GattLocalCharacteristicResult result =
                await ServiceProvider.Service.CreateCharacteristicAsync(
                    guid_Buts_TxCharacteristic,
                    PlainNotifyParameters);

            // Grab the characterist object from the service set it to the Bluetooth Uart property which is of a specfic Characteristic type
            GattLocalCharacteristic baseButsTx = null;
            GattServicesHelper.GetCharacteristicsFromResult(result, ref baseButsTx);

            if (baseButsTx != null)
            {
                ButsTx = new Characteristics.HeartRateMeasurementCharacteristic(baseButsTx, this);
            }
        }
    }
}
