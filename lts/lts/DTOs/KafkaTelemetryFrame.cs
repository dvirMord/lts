using System.Text.Json;
using System.Text.Json.Serialization;

namespace lts.DTOs;
public class KafkaTelemetryFrame
{
    [JsonPropertyName("PrecisionTimeStamp")]
    public string PrecisionTimeStamp { get; set; } = string.Empty;

    [JsonPropertyName("PlatformTailNumber")]
    public string PlatformTailNumber { get; set; } = string.Empty;

    [JsonPropertyName("PlatformHeadingAngle")]
    public string PlatformHeadingAngle { get; set; } = "0";

    [JsonPropertyName("PlatformPitchAngle")]
    public string PlatformPitchAngle { get; set; } = "0";

    [JsonPropertyName("PlatformRollAngle")]
    public string PlatformRollAngle { get; set; } = "0";

    [JsonPropertyName("SensorLatitude")]
    public string SensorLatitude { get; set; } = "0";

    [JsonPropertyName("SensorLongitude")]
    public string SensorLongitude { get; set; } = "0";

    [JsonPropertyName("SensorTrueAltitude")]
    public string SensorTrueAltitude { get; set; } = "0";

    [JsonPropertyName("SensorRelativeAzimuthAngle")]
    public string SensorRelativeAzimuthAngle { get; set; } = "0";

    [JsonPropertyName("SensorRelativeElevationAngle")]
    public string SensorRelativeElevationAngle { get; set; } = "0";

    [JsonPropertyName("FrameCenterLatitude")]
    public string? FrameCenterLatitude { get; set; }

    [JsonPropertyName("FrameCenterLongitude")]
    public string? FrameCenterLongitude { get; set; }

    [JsonPropertyName("FrameCenterElevation")]
    public string? FrameCenterElevation { get; set; }

    [JsonPropertyName("MissionID")]
    public string? MissionID { get; set; }

    [JsonPropertyName("PlatformDesignation")]
    public string? PlatformDesignation { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalFields { get; set; }

    public string ToFormattedSummary(int partition, long offset)
    {
        return $"""
        ==================== [TELEMETRY FRAME] ====================
        Partition:     {partition} | Offset: {offset}
        Timestamp:     {PrecisionTimeStamp}
        Platform:      {PlatformTailNumber} (Designation: {PlatformDesignation ?? "N/A"}, Mission: {MissionID ?? "N/A"})
        UAV Position:  Lat: {SensorLatitude}°, Lon: {SensorLongitude}°, Alt: {SensorTrueAltitude} m
        UAV Attitude:  Heading: {PlatformHeadingAngle}°, Pitch: {PlatformPitchAngle}°, Roll: {PlatformRollAngle}°
        Gimbal Angles: Azimuth/Pan: {SensorRelativeAzimuthAngle}°, Elevation/Tilt: {SensorRelativeElevationAngle}°
        Target Center: Lat: {FrameCenterLatitude ?? "N/A"}, Lon: {FrameCenterLongitude ?? "N/A"}, Elev: {FrameCenterElevation ?? "N/A"} m
        ===========================================================
        """;
    }
}
