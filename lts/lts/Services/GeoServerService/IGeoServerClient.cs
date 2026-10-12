using System.Threading;
using System.Threading.Tasks;
using lts.DTOs;

namespace lts.Interfaces;

public interface IGeoServerClient
{
    Task<RaycastResponseDto?> CalculateIntersectionAsync(RaycastRequestDto request, CancellationToken ct = default);
}