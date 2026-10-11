using System.ComponentModel.DataAnnotations;
using static lts.Constants.Constatns;
namespace lts.DTOs;
public class PartitionRequest
{
    [Range(partitionDTOs.Min, partitionDTOs.Max, ErrorMessage = partitionDTOs.ErrorMessage)]
    public int Partition { get; set; }
}
