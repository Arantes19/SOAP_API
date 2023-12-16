namespace DeviceService.Models
{
    /// <summary>
    /// This class represents a Device
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Device's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Device's Name
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Device's State
        /// </summary>
        public bool? State { get; set; } = false;

        /// <summary>
        /// Device's Value
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// Room's Id
        /// </summary>
        public int HouseId { get; set; }
    }
}