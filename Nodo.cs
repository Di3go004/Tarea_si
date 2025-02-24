using System;



namespace Tarea_4
{

  public unsafe struct Node
  {
    public int Id;
    public fixed char Repuesto[50];
    public fixed char Detalles[100];
    public double Costo;

    public Node* Siguiente;

    public Node(int id, string repuesto, string detalles, double costo)
    {
      Id = id;
      Costo = costo;
      Siguiente = null;

      // fixed es una palabra clave que crea un puntero a un tipo de dato especifico, en este caso un char
      fixed (char* ptr = Repuesto)
        repuesto.AsSpan().CopyTo(new Span<char>(ptr, 50));
      fixed (char* ptr = Detalles)
        detalles.AsSpan().CopyTo(new Span<char>(ptr, 100));
    }
        // Sobrecarga del metodo To String para poder mostrar los datos del nodo
    public override string ToString()
    {
      fixed (char* ptrRepuesto = Repuesto, ptrDetalles = Detalles)
      {
        return $"ID: {Id}, Repuesto:{new string(ptrRepuesto)}, Detalles: {new string(ptrDetalles)}, Costo:{Costo} ";
      }
    }
  }
}
