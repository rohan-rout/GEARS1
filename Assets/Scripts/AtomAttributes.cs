public class AtomAttributes{
    public string iatom;

    public double[] rr0 = new double[3];
    public double[] pos = new double[3];

    public int[] cellId = new int[3];
    public int cellNumber;
    public int stress;

    public AtomAttributes(string atomName,double[] rr)
    {
        this.iatom = atomName;
        this.rr0 = rr;
        for(int index = 0; index < 3; index++)
        {
            this.pos[index] = -1.0;
            this.cellId[index] = -1;
        }
        this.cellNumber = -1;
    }
}
