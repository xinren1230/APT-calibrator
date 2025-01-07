using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

using SharpGL;
namespace APT_calibrator_tool
{
    class myReadObj
    {
        public myReadObj()
        {

        }
        public class POINT3
        {
            public double X;
            public double Y;
            public double Z;
        };
        public class WenLi
        {
            public double TU;
            public double TV;
        };
        public class FaXiangLiang
        {
            public double NX;
            public double NY;
            public double NZ;
        };
        public class Mian
        {
            public int[] V = new int[3];
            public int[] T = new int[3];
            public int[] N = new int[3];
        };
        public class Model
        {

            public List<POINT3> V = new List<POINT3>();//V：代表顶点。格式为V X Y Z，V后面的X Y Z表示三个顶点坐标。浮点型
            public List<WenLi> VT = new List<WenLi>();//表示纹理坐标。格式为VT TU TV。浮点型
            public List<FaXiangLiang> VN = new List<FaXiangLiang>();//VN：法向量。每个三角形的三个顶点都要指定一个法向量。格式为VN NX NY NZ。浮点型
            public List<Mian> F = new List<Mian>();//F：面。面后面跟着的整型值分别是属于这个面的顶点、纹理坐标、法向量的索引。
                                                   //面的格式为：f Vertex1/Texture1/Normal1 Vertex2/Texture2/Normal2 Vertex3/Texture3/Normal3
        }

        public Model mesh = new Model();
        public float movX;
        public float movY;
        public float movZ;
        public float xRotate;
        public float yRotate;
        public float x;
        public float y;

        //放缩参数
        public static float scale;
        //显示列表
        public uint showFaceList;

        public int YU = 1;

        public void loadFile(String fileName)
        {
            // Mian[] f;
            //POINT3[] v;
            //FaXiangLiang[] vn;
            //WenLi[] vt;

            StreamReader objReader = new StreamReader(fileName);
            ArrayList al = new ArrayList();
            string texLineTem = "";
            while (objReader.Peek() != -1)
            {
                texLineTem = objReader.ReadLine();
                if (texLineTem.Length < 2) continue;
                if (texLineTem.IndexOf("v") == 0)
                {
                    if (texLineTem.IndexOf("t") == 1)//vt 0.581151 0.979929 纹理
                    {
                        string[] tempArray = texLineTem.Split(' ');
                        WenLi vt = new WenLi();
                        vt.TU = double.Parse(tempArray[1]);
                        vt.TV = double.Parse(tempArray[2]);
                        mesh.VT.Add(vt);
                    }
                    else if (texLineTem.IndexOf("n") == 1)//vn 0.637005 -0.0421857 0.769705 法向量
                    {
                        string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                        FaXiangLiang vn = new FaXiangLiang();
                        vn.NX = double.Parse(tempArray[1]);
                        vn.NY = double.Parse(tempArray[2]);
                        if (tempArray[3] == "\\")
                        {
                            texLineTem = objReader.ReadLine();
                            vn.NZ = double.Parse(texLineTem);
                        }
                        else vn.NZ = double.Parse(tempArray[3]);

                        mesh.VN.Add(vn);
                    }
                    else
                    {//v -53.0413 158.84 -135.806 点
                        string[] tempArray = texLineTem.Split(' ');
                        POINT3 v = new POINT3();
                        v.X = double.Parse(tempArray[1]);
                        v.Y = double.Parse(tempArray[2]);
                        v.Z = double.Parse(tempArray[3]);
                        mesh.V.Add(v);
                    }
                }
                else if (texLineTem.IndexOf("f") == 0)
                {
                    //f 2443//2656 2442//2656 2444//2656 面
                    string[] tempArray = texLineTem.Split(new char[] { '/', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                    Mian f = new Mian();
                    int i = 0;
                    int k = 1;
                    while (i < 3)
                    {
                        if (mesh.V.Count() != 0)
                        {
                            f.V[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.VT.Count() != 0)
                        {
                            f.T[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        if (mesh.VN.Count() != 0)
                        {
                            f.N[i] = int.Parse(tempArray[k]) - 1;
                            k++;
                        }
                        i++;
                    }
                    mesh.F.Add(f);

                }
            }


        }

        public uint createListFace(ref SharpGL.OpenGL gl)
        {
            gl.NewList(showFaceList, OpenGL.GL_COMPILE);
            if (mesh.V.Count() == 0) return 119;
            for (int i = 0; i < mesh.F.Count(); i++)
            {
                gl.Begin(OpenGL.GL_TRIANGLES);                          // 绘制三角形
                if (mesh.VT.Count() != 0) gl.TexCoord(mesh.VT[mesh.F[i].T[0]].TU, mesh.VT[mesh.F[i].T[0]].TV);  //纹理    
                if (mesh.VN.Count() != 0) gl.Normal(mesh.VN[mesh.F[i].N[0]].NX, mesh.VN[mesh.F[i].N[0]].NY, mesh.VN[mesh.F[i].N[0]].NZ);//法向量
                gl.Vertex(mesh.V[mesh.F[i].V[0]].X / YU, mesh.V[mesh.F[i].V[0]].Y / YU, mesh.V[mesh.F[i].V[0]].Z / YU);        // 上顶点

                if (mesh.VT.Count() != 0) gl.TexCoord(mesh.VT[mesh.F[i].T[1]].TU, mesh.VT[mesh.F[i].T[1]].TV);  //纹理
                if (mesh.VN.Count() != 0) gl.Normal(mesh.VN[mesh.F[i].N[1]].NX, mesh.VN[mesh.F[i].N[1]].NY, mesh.VN[mesh.F[i].N[1]].NZ);//法向量
                gl.Vertex(mesh.V[mesh.F[i].V[1]].X / YU, mesh.V[mesh.F[i].V[1]].Y / YU, mesh.V[mesh.F[i].V[1]].Z / YU);        // 左下

                if (mesh.VT.Count() != 0) gl.TexCoord(mesh.VT[mesh.F[i].T[2]].TU, mesh.VT[mesh.F[i].T[2]].TV);  //纹理
                if (mesh.VN.Count() != 0) gl.Normal(mesh.VN[mesh.F[i].N[2]].NX, mesh.VN[mesh.F[i].N[2]].NY, mesh.VN[mesh.F[i].N[2]].NZ);//法向量
                gl.Vertex(mesh.V[mesh.F[i].V[2]].X / YU, mesh.V[mesh.F[i].V[2]].Y / YU, mesh.V[mesh.F[i].V[2]].Z / YU);        // 右下
                gl.End();                                        // 三角形绘制结束    
            }
            gl.EndList();
            return showFaceList;
        }
    }
}
