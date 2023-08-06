using miranaSolution.DTOs.Core.CurrentlyReading;

namespace miranaSolution.API.ViewModels.CurrentlyReadings;

public record ApiGetCurrentlyReadingBooksResponse(
    List<CurrentlyReadingVm> CurrentlyReadings);