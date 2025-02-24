using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.IO;

namespace Tarea_4
{
  public unsafe class ListaCircular
  {
    private Node* head;

    public ListaCircular()
    {
      head = null;
    }

    public void Insertar(int id, string repuesto, string detalles, double costo)
    {
      Node* nuevoNodo = (Node*)Marshal.AllocHGlobal(sizeof(Node));
      *nuevoNodo = new Node(id, repuesto, detalles, costo);

      if(head == null)
      {
        head = nuevoNodo;
        head->Siguiente = head; // se apunta a si mismo 
      }
      else
      {
        Node* temp = head;
        // recorremos la lista hasta llegar al ultimo nodo
        while ( temp->Siguiente != head)
        {
          temp = temp->Siguiente;
        }
        // insertamos el nuevo noo alfinal de la lista
        temp->Siguiente = nuevoNodo;
        nuevoNodo->Siguiente = head;

      }
    }

    public void Eliminar(int id)
    {
      // si la lista esta vacia no hay nada que se pueda eliminar
      if(head == null) return;
      // si el nodo a eliminar es la cabeza de la lista 
      if(head->Id == id && head->Siguiente == head)
      {
        Marshal.FreeHGlobal((IntPtr)head);
        head = null;
        return;
      }

      Node* temp = head;
      Node* prev = null;
      do
      {
        // si el nodo a eliminar es la cabeza de la lista
        if(temp->Id == id)
        {
          if(prev != null)
          {
            prev->Siguiente = temp->Siguiente;
          }
          else
          {
            Node* last = head;
            while(last->Siguiente != head)
            {
              last = last->Siguiente;
            }
            head = head->Siguiente;
            last->Siguiente = head;
          }

          //liberamos la memoria del nodo eliminado
          Marshal.FreeHGlobal((IntPtr)temp);
          return;
        }
        prev = temp;
        temp = temp->Siguiente;
      }while(temp != head);
    }

    public void Mostrar()
    {
      if (head == null)
      {
        Console.WriteLine("Lista vacia");
        return;
      }

      Node* temp = head;
      do
      {
        Console.WriteLine(temp->ToString());
        temp = temp->Siguiente;
      } while(temp != head);
    }


    // metodo para generar el codigo graphiz

    public string GenerarGraphviz()
    {

      // si la lista esta vacia, generamos un solo dono con Null
      if(head == null)
      {
        return "digraph6 {\n node[shape=record];\n Null [label = \"{NULL}\"];\n}\n";
        
      }
      // Iniciamos el código Graphviz
        var graphviz = "digraph G {\n";
        graphviz += "    node [shape=record];\n";
        graphviz += "    rankdir=LR;\n";
        graphviz += "    subgraph cluster_0 {\n";
        graphviz += "        label = \"Lista Circular (Diego Gonzalez)\";\n";

      // insertar sobre los nodos de la lista y contruir la representacion grafica
      Node* actual = head;
      int index = 0;

    do
    {
        string repuesto = new string(actual->Repuesto);
        string detalles = new string(actual->Detalles);
        graphviz += $"        n{index} [label = \"{{<data> ID: {actual->Id} \\n Repuesto: {repuesto} \\n Detalles: {detalles} \\n Costo: {actual->Costo} \\n Siguiente: }}\"];\n";
        actual = actual->Siguiente;
        index++;
    } while (actual != head);

    // Conectar los nodos
    actual = head;
    for (int i = 0; i < index; i++)
    {
        graphviz += $"        n{i} -> n{(i + 1) % index};\n";
        actual = actual->Siguiente;
    }

    graphviz += "    }\n";
    graphviz += "}\n";
    return graphviz;
    }

//generar el reporte de grapiz
        public void GenerarReporteGraphviz(string dotFilePath, string pngFilePath)
        {
            string graphvizCode = GenerarGraphviz();
            File.WriteAllText(dotFilePath, graphvizCode);

            ProcessStartInfo startInfo = new ProcessStartInfo("dot");
            startInfo.Arguments = $"-Tpng {dotFilePath} -o {pngFilePath}";
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
            }
        }


// Destructor de la clase
    ~ListaCircular()
    {

      //liberamos la memoria de todos los nodos de la lista
      if(head == null) return;

      Node* temp = head;
      do{
        Node* siguiente = temp->Siguiente;
        Marshal.FreeHGlobal((IntPtr)temp);
        temp = siguiente;

      }while(temp != head);
    }

  }
}