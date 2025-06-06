public class Registration
{
    public int VsId { get; set; }
    public string Algorithm { get; set; }
    public string Revision { get; set; }
    public bool IsSample { get; set; }
    public string[] Conformances { get; set; }
    public string[] Direction { get; set; }
    public int[] KeyLen { get; set; }
}