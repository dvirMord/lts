namespace lts.DTOs;

public class RaycastRequestDto
{
    public double SensorLatitude { get; set; }
    public double SensorLongitude { get; set; }
    public double SensorAltitudeMeters { get; set; }
    public double PlatformHeadingDeg { get; set; }
    public double PlatformPitchDeg { get; set; }
    public double PlatformRollDeg { get; set; }
    public double GimbalAzimuthDeg { get; set; }
    public double GimbalElevationDeg { get; set; }
    public double GimbalRollDeg { get; set; }
    public double? FrameCenterLatitude { get; set; }
    public double? FrameCenterLongitude { get; set; }
    public double? FrameCenterElevation { get; set; }
}

public class RaycastResponseDto
{
    public bool HasIntersection { get; set; }
    public double GroundLatitude { get; set; }
    public double GroundLongitude { get; set; }
    public double GroundAltitudeMeters { get; set; }
    public double SlantRangeMeters { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
}