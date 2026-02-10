using System.ComponentModel.DataAnnotations;

namespace GenValAppRunner.DTO
{
    public class Registration
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "VsId must be a positive integer.")]
        public int VsId { get; set; }
        [Required]
        [MinLength(2)]
        public string Algorithm { get; set; }
        public string Revision { get; set; }
        public bool IsSample { get; set; }
        public string[] Conformances { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one direction must be specified.")]
        public string[] Direction { get; set; }
        public string[] KwCipher {get; set;}
        public PayloadLength[] PayloadLen { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least one key length must be specified.")]
        public int[] KeyLen { get; set; }
    }
}