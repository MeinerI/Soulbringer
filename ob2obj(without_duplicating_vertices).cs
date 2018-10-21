// ����� � ����������� *.ob ������ ��������� � ����� ����� � c��������������� *.exe

// � *.obj-������ ����������� ������������ ������ , �� �������� uv

//����������������������������������������������������������������������������������������������������������������������������������������������������

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//����������������������������������������������������������������������������������������������������������������������������������������������������

sealed class Test
{

//����������������������������������������������������������������������������������������������������������������������������������������������������

		static string[] readText; // ������ ��� ������ �� �����
		static int VTXS, POLY4, POLY3; // ������ ���������� ������ � �.�.

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

//	���� ����� ��������� ��� ���� vt u v

//	������������ ��������� ��� �����-��������, ������������� �� �����.
//	static SortedDictionary<int, string> vt_uv = new SortedDictionary<int, string>(); 

//	������������ ��������� ��� �����-��������, ������������� �� ����� �� ������ ���������� IComparer<T>.
		static SortedList<int, string> vt_uv = new SortedList<int, string>();

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

		static List<string> WriteObjList = new List<string>(); // ���� ����� ��������� ��� ������ 
		static List<string> FaceList = new List<string>();
		static List<string> VertexList = new List<string>();

		static HashSet<string> newmtl = new HashSet<string>();	// ��� *.mtl �����
		static List<string>		 usemtl = new List<string>();

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

		static int iii = 1 ; // "�������" "����������" ��������� ��������
		static int vii;
		static string[] VertexArray;	//	������ ��� ������ � ��������� (��� ������������)

//����������������������������������������������������������������������������������������������������������������������������������������������������
//����������������������������������������������������������������������������������������������������������������������������������������������������

    public static int Main(string[] args)
    {
				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// ����� ������ �������				// ���� ��� obj ��� �� ����� ����� ��				// ��� � ���� ������ ��������
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// ���� ���������� � ������� �������� (������� ����� ������)
				DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

				// ����� ���� � ���� *.ob ������ � ������� ��������
				FileInfo[] Files = d.GetFiles("*.ob");
				
				///////////////////////////////////////////////////////////////////////////////////////////////////////////////
				
				foreach (FileInfo file in Files) // ��� ������� �����
				{
						// ������ ��� ������ � ������
						readText = File.ReadAllLines(file.Name);

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////

						VTXS  = int.Parse(readText[2].Split(' ')[1]);	//	�������
						POLY4 = int.Parse(readText[4].Split(' ')[1]);	//	��������
						POLY3 = int.Parse(readText[5].Split(' ')[1]);	//	������������
						
						VertexArray = new string[VTXS];

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////

// ������� ���� �� ������� - � �������� ����������� �� *.obj � *.ob 
// �������� ��� ����������, ������ ��� ��������� ��������� - �������� ������� ����� ��� �������/��������

#if OB2OBJ
						WriteObjList.Add("#" + readText[0]);
#endif
//					string mtlFileName = "mtllib " + file.Name + ".mtl" + Environment.NewLine; 
						string mtlFileName = "mtllib " + Path.GetFileNameWithoutExtension(file.Name) + "_mtl.mtl" + Environment.NewLine; 
						WriteObjList.Add(mtlFileName); // ������� ������, ���������� ���� � ������������ ���������� ����������
#if OB2OBJ
						// ������������ ������ ������ ����� � ������ ��� ������ �����
						for ( int s = 1 ; s < 10 ; s++ ) WriteObjList.Add( "#" + readText[s] );
#endif
						/////////////////////
						// ������� v x y z /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////

						int VTXS10 = 10; // ���������� ������ � ����� *.ob - ������ ���������� � 11-�� ������ = 10 ������ � �������

						for ( int vi = 0 ; vi < VTXS ; vi++ )		// ������ VTXS ����� �������		//		x,y,z
						{
								int[] vrtx_ar = readText[VTXS10 + vi].Split(',').Select(n => Convert.ToInt32(n)).ToArray();		//	x y z
							//VertexArray[vi] = "v " + ((-1)*vrtx_ar[1]).ToString() + " " + vrtx_ar[2].ToString() + " " + ((-1)*vrtx_ar[0]).ToString(); // v -(y) z -(x)
								
							VertexArray[vi] = "v " + ((-1)*vrtx_ar[0]*0.005f).ToString() + " " + (vrtx_ar[2]*0.005f).ToString() + " " + (vrtx_ar[1]*0.005f).ToString();	//	-x z y
							/*	
							VertexArray[vi] = "v " 
							+ Math.Round((double)(-1)*vrtx_ar[0]*0.005f,2).ToString() 
							// ((-1)*vrtx_ar[0]*0.005f).ToString() 
							+ " " 
							// (vrtx_ar[2]*0.005f).ToString() 
							+ Math.Round((double)(-1)*vrtx_ar[0]*0.005f,2).ToString() 
							+ " " 
							+ Math.Round((double)vrtx_ar[1]*0.005f,2).ToString();
							//(vrtx_ar[1]*0.005f).ToString();	//	-x z y
							*/
								VertexList.Add( VertexArray[vi] ); // �������� � ������ - ������������ ������ "������/������������" ������
						}

						// WriteObjList.Add(""); // ������ ����� "�������" � "�������" ���������

						////////////////////////
						// f 1 2 3 (4) //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						////////////////////////

						usemtl.Add("0"); usemtl.Add("0"); // ��� ����� �� ����������� ������ usemtl � // ��� �� �������� :D

						             int POLY_index = 0; // ������� � ���� (������/�������� ������� �����) �������� �� ���� ������� � �������
						if (POLY4 > 0) { POLY_index = VTXS10 + VTXS + 7 ;                   Method(4, POLY_index); }
						if (POLY3 > 0) { POLY_index = VTXS10 + VTXS + 6 + (POLY4 * 5) + 3 ; Method(3, POLY_index); }

						/////////////////////
						// ������� v x y z /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////

						foreach (var vert in VertexList)	// ��������� ��������� ������ � ������ WriteObjList 
						{
								WriteObjList.Add(vert);  // ��� �������� ����� "������������" UV "��-��" ������
						}

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if OB2OBJ
						WriteObjList.Add(""); // �������� ����� ����� "�������" ������
						WriteObjList.Add("#OBJS");
						WriteObjList.Add(""); 
						WriteObjList.Add("#" + readText [ VTXS10 + VTXS + 3 ] ); 
#endif
						WriteObjList.Add(""); // �������� ����� ����� OBJS v,f3,f4,0,0,0,0,0

						///////////////////////////////////
						// ���������� ���������� vt u v //////////////////////////////////////////////////////////////////////////////////
						//////////////////////////////////

						// WriteObjList.Add(""); // ��������� ����� ����� ������

						foreach (var item in vt_uv) 	    // ��������� uv � ������ WriteObjList
						{
							//���������� �� � ��������� ������, ������ �� ������ ��� ������
								WriteObjList.Add(item.Value);	// ������������� �� ������� �� ����������� ������ ���� vt_u_v
							//Console.WriteLine(item.Value);
						}

						WriteObjList.Add(""); 

						////////////////////////
						// f 1 2 3 (4) //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						////////////////////////

						foreach (var face in FaceList)	// ��������� faces � ������ WriteObjList
						{
								WriteObjList.Add(face);
						}

#if OB2OBJ
						WriteObjList.Add(""); 
						WriteObjList.Add("#FACE4"); 
						WriteObjList.Add(""); 
#endif
						/////////////////////////////
						// ������� ����� obj � mtl ////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////////////

						string WriteFilePath = Path.GetFileNameWithoutExtension(file.Name) + "_obj.obj";
						string[] OBJ_write_Text = WriteObjList.ToArray();
						File.WriteAllLines(WriteFilePath, OBJ_write_Text);

						//----------------------------------------------------------------------------------------------------------------------------------------------------

						List<string> mtlList = new List<string>(); 

						foreach ( string nmat in newmtl )
						{
								mtlList.Add("newmtl " + nmat);           //	��������� ������ newmtl ##
								mtlList.Add("map_Kd " + nmat + ".png");  //	��������� ������ map_Kd ##.dds
								mtlList.Add("");
						}

						string[] MTL_write_Text = mtlList.ToArray();
						string WriteMTLPath = Path.GetFileNameWithoutExtension(file.Name) + "_mtl.mtl";
						File.WriteAllLines(WriteMTLPath, MTL_write_Text);

						//----------------------------------------------------------------------------------------------------------------------------------------------------

						// ������� ��� ���������, � �� ��� ������������� 

						VertexList.Clear();	
						vt_uv.Clear();	
						FaceList.Clear(); 
						newmtl.Clear();	
						mtlList.Clear();	
						usemtl.Clear(); 
						WriteObjList.Clear(); 

						iii = 1; // ��� ����� ������ - ������� ������� ����� ������

						//---------------------------------------------------------------------------------------------------------------------------------------
				}

				if ( Files.Length > 0 ) Console.WriteLine("Processed {0} file(s)", Files.Length);
				else Console.WriteLine("� ��������� �������� - ��� ������ � ��������!");

				return 0;
		} 

// ����� Main()//�������������������������������������������������������������������������������������������������������������������������������������
//����������������������������������������������������������������������������������������������������������������������������������������������������
//����������������������������������������������������������������������������������������������������������������������������������������������������

		private static void Method(int poly_number, int POLYindex)
		{
				string[] POLYwords = new string[poly_number]; // 3 ��� 4 ������ �������� ������

				                    int POLY = 0; string polySTR = ""; // =_=
				if (poly_number == 4) { POLY = POLY4;    polySTR = "#POLY4"; } 
				if (poly_number == 3) { POLY = POLY3;    polySTR = "#POLY3"; } 

#if OB2OBJ
				FaceList.Add(polySTR); 
				FaceList.Add(""); 
#endif
				for ( int i = POLYindex ; i < POLYindex + POLY * 5; i+=5 ) // ������ ������ � 1,2,3(,4)
				{
						// ���� [i] ������ ������, �������� �������� n , ������� ����������� � Int32 � �������� �� � ������ ?

						int[] POLYface = readText[i].Split(',' , ' ').Select(n => Convert.ToInt32(n) + 1 ).ToArray();	// (0)+1 (1)+1 (2)+1 ((3)+1)

						for ( int k = 0 ; k < POLYface.Length ; k++ ) 
						{
								POLYwords[k] = POLYface[k] + "/" + POLYface[k];  // 1/1 2/2 3/3 (4/4)

								// ���� �������� ������ // ������ �������� ���� �����
								int[] POLYuvs = readText[i + 1].Split(',').Select(uvs => Convert.ToInt32(uvs)).ToArray();	//	1 2 3 4 5 6 (7 8)

								//double u = Math.Round((double)POLYuvs[k]/255f, 2);									// 1 2 3 // 1 2 3 4
								float u = POLYuvs[k]/255f;
								
								//double v = Math.Round((double)POLYuvs[k + poly_number]/255f, 2);		// 4 5 6 // 5 6 7 8
								float v = POLYuvs[k + poly_number]/255f;

								string vtuv = "vt" + "\t" + u + "\t" + v; // vt 1 4 // vt 1 5 

								if ( !vt_uv.ContainsKey(POLYface[k]) ) // ���� ����� 1 ���, �� ����������� ��� ��������
								{
									vt_uv[POLYface[k]] = vtuv; 
#if OB2OBJ
									Console.WriteLine( POLYface[k] + "\t" + "\t" + vt_uv[POLYface[k]] ); 
#endif
								}
								else // �����, ���� ���� ����, �� ������ ����� ���� � ����������� ��� "�����" +1 ��������
								{
									vii = VTXS + iii++ ;
									vt_uv.Add( vii , vtuv );	// ��������� ����� ���� (� ������) ���������� ���������
									POLYwords[k] = POLYface[k] + "/" + vii;	//	����� ������� (��������)/����� uv
									// VertexList.Add(VertexArray[POLYface[k]-1]); // ��������� �������� �������, -1 ��� ������ ��� ���� �� ��������� face �� �������
#if OB2OBJ
									Console.WriteLine( POLYface[k] + "\t" + vii + "\t" + vt_uv[POLYface[k]] ); // ������ ����� �������� ��� ut_uv
									//Console.WriteLine( vii + "\t" + "\t" + vii ); // ����� ���� ������/���������
#endif
								}
						}

//----------------------------------------------------------------------------------------------------------------------------------------------------

						string POLYmat     = readText[i+2];		// usemtl � in *.obj
						string render_side = readText[i+3];

						//FaceList.Add("usemtl " + POLYmat); // ������ ���� ��� // ������ ����������� ���������� ������

// usemtl //������� ����� �� ����������� // ������

						usemtl.Add(POLYmat);	// ��������� POLYmat ��������
						if (POLYmat != usemtl[usemtl.Count - 2]) // ���� �� ����� �����������, �� �����
								FaceList.Add("usemtl " + POLYmat);

////////////
						if (!newmtl.Contains(POLYmat)) newmtl.Add(POLYmat);    // newmtl � in *.mtl
//////face
						FaceList.Add("f "+ string.Join(" ", POLYwords));       // f + 1/1 2/2 3/3 (4/4)
//////#mat 
#if OB2OBJ
						FaceList.Add("#" + POLYmat); 
						FaceList.Add("#" + render_side); 
#endif
						//FaceList.Add(""); 
				}
				
//----------------------------------------------------------------------------------------------------------------------------------------------------

//					WriteObjList.Add(""); // �������� ����� ����� "�������" ��������

		} // ����� ������ Method(int poly_number, int POLYindex)

//����������������������������������������������������������������������������������������������������������������������������������������������������
//����������������������������������������������������������������������������������������������������������������������������������������������������
//����������������������������������������������������������������������������������������������������������������������������������������������������

} // ����� ������
