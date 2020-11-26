[System.Serializable]
public class Cycle {

    public int[] BoxesIdForCycle { get; set; }
    public int[] ConveyorsInCycle { get; set; }
    public int ChangeDirectoryConveyorId { get; set; } = 0;
}
