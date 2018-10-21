// файлы с расширением *.ob должны находится в одной папке с cкомпилированным *.exe

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

sealed class Test
{

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

		static List<string> WritePLYList = new List<string>(); // сюда будем добавлять все строки 
		static string[] readText; // хранит все строки из файла
		static int VTXS, POLY4, POLY3; // хранят количество вершин и т.д.
		static string[] uv; // 6 0,09163 0,565323 0,109197 0,565733 0,10888 0,602539
		static string[] FaceList;
		static float[] uv_ar;

		static SortedList<int, string> texture_page_list = new SortedList<int, string>();

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

    public static int Main(string[] args)
    {
    		int VTXS10 = 10; // координаты вершин в файле *.ob - всегда начинаются с 11-ой строки = 10 индекс в массиве

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// точки вместо запятых				// хотя для obj это не важно вроде бы				// как и табы вместо пробелов
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

				FileInfo[] Files = d.GetFiles("*.ob");
				
				foreach (FileInfo file in Files)
				{
						readText = File.ReadAllLines(file.Name);			// читаем все строки из файла

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////

						VTXS  = int.Parse(readText[2].Split(' ')[1]);	//	вершины
						POLY4 = int.Parse(readText[4].Split(' ')[1]);	//	квадраты
						POLY3 = int.Parse(readText[5].Split(' ')[1]);	//	треугольники

						string[] VertexArray = new string[VTXS]; // количество строк с вершинами

						//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

						// добавить комменты с именами текстур

						int POLY = 0; int POLYindex = 0; int j=0; // начиная с этой (строки/элемента массива строк) проходим по всем строкам с фейсами

						if (POLY4 > 0) { POLYindex = VTXS10 + VTXS + 7 ; POLY = POLY4 ; }

						for ( int i = POLYindex ; i < POLYindex + POLY * 5; i+=5 ) // каждую строку с номером текстуры
						{
							if (!texture_page_list.ContainsValue(readText[i + 2]))
							{
										texture_page_list.Add ( j , readText[i + 2] ) ; 
										j++;
							}
						}

						// if(hashSet.Add(item)) orderList.Add(item);

						if (POLY3 > 0) { POLYindex = VTXS10 + VTXS + 6 + (POLY4 * 5) + 3 ; POLY = POLY3 ; }

						for ( int i = POLYindex ; i < POLYindex + POLY * 5; i+=5 ) // каждую строку с номером текстуры
						{
							if (!texture_page_list.ContainsValue(readText[i + 2]))
							{
										texture_page_list.Add ( j , readText[i + 2] ) ; 
										j++;
							}
						}

						//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

						WritePLYList.Add("ply");	//	магическое слово 

						WritePLYList.Add("format ascii 1.0");	//	единственная версия формата

						//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

						foreach (var item in texture_page_list)
						{
							WritePLYList.Add("comment TextureFile " 
							// + file.Name + "_" 
							+ item.Value + ".png");
						}

						//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

//					WritePLYList.Add("");

						WritePLYList.Add("element vertex " + VTXS);	//	количество вершин

						WritePLYList.Add("property float x");
						WritePLYList.Add("property float y");
						WritePLYList.Add("property float z");

//					WritePLYList.Add("");

						WritePLYList.Add("element face " + ( POLY4*2 + POLY3 )); // общее количество треугольных граней

						WritePLYList.Add("property list uchar int vertex_indices"); 
						// "vertex_indices" is a list of ints (где uchar - их количество? , а int - размер значений?)

					//WritePLYList.Add("property list ushort float texcoord");
						WritePLYList.Add("property list uchar float texcoord");
						WritePLYList.Add("property int texnumber");

						WritePLYList.Add("end_header"); // конец заголовка
/*
						WritePLYList.Add("");
						WritePLYList.Add("comment start of vertex list");	//	коммент - начало списка вершин
						WritePLYList.Add("");
*/
						/////////////////////
						// ВЕРШИНЫ v x y z /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////

//					WritePLYList.Add("");

						for ( int vi = 0 ; vi < VTXS ; vi++ )		// читать VTXS строк массива		//		x,y,z
						{
								int[] vrtx_ar = readText[VTXS10 + vi].Split(',').Select(n => Convert.ToInt32(n)).ToArray();		//	x y z

								//VertexArray[vi] = ((-1)*vrtx_ar[1]).ToString() + " " + vrtx_ar[2].ToString() + " " + ((-1)*vrtx_ar[0]).ToString(); // -y z -x
								//VertexArray[vi] = ((-1)*vrtx_ar[0]).ToString() + " " + vrtx_ar[2].ToString() + " " + vrtx_ar[1].ToString();	//	-x y z

/*это работает
								VertexArray[vi] = Math.Round(-1*vrtx_ar[0]*0.005f , 2).ToString() + " " 
								                + Math.Round((double)vrtx_ar[2]*0.005f , 2).ToString() + " " 
								                + Math.Round((double)vrtx_ar[1]*0.005f , 2).ToString();	//	-x z y
*/
// а это без округления

								VertexArray[vi] = (-1*vrtx_ar[0]*0.005f).ToString() + " " 
								                + (vrtx_ar[2]*0.005f).ToString() + " " 
								                + (vrtx_ar[1]*0.005f).ToString();	//	-x z y


								WritePLYList.Add( VertexArray[vi] ); 
						}

//					WritePLYList.Add("");

						////////////////////////
						// f 1 2 3 (4) //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						////////////////////////

						uv = new string[POLY3 + POLY4];

						FaceList = new string[POLY3 + POLY4];

						int POLY_index = 0; // начиная с этой (строки/элемента массива строк) проходим по всем строкам с фейсами

						if (POLY4 > 0) { POLY_index = VTXS10 + VTXS + 7 ; Method(4, POLY_index); /*WritePLYList.Add("");*/ } 

						if (POLY3 > 0) { POLY_index = VTXS10 + VTXS + 6 + (POLY4 * 5) + 3 ; Method(3, POLY_index); }

//					WritePLYList.Add("");

						/////////////////////////////
						// СОЗДАЁМ ФАЙЛЫ obj и mtl ////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////////////

						string WriteFilePath = Path.GetFileNameWithoutExtension(file.Name) + "_ply.ply";
						string[] OBJ_write_Text = WritePLYList.ToArray();
						File.WriteAllLines(WriteFilePath, OBJ_write_Text);

						//----------------------------------------------------------------------------------------------------------------------------------------

						// очищаем все коллекции, а то они накапливаются 

						WritePLYList.Clear(); 
						//
						texture_page_list.Clear();

						//----------------------------------------------------------------------------------------------------------------------------------------
				}

				if ( Files.Length > 0 ) Console.WriteLine("Processed {0} file(s)", Files.Length);
				else Console.WriteLine("В выбранном каталоге - нет файлов с моделями!");

				return 0;
		}

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

		private static void Method(int poly_number, int POLYindex)
		{
				                    int POLY = 0; string polySTR = ""; // =_=
				if (poly_number == 3) { POLY = POLY3;    polySTR = "3 "; } 
				if (poly_number == 4) { POLY = POLY4;    polySTR = "3 "; } 

				for ( int i = POLYindex ; i < POLYindex + POLY * 5 ; i+=5 ) // каждую строку с 1,2,3(,4)
				{
						uv_ar = readText[i + 1].Split(',').Select(n => (Convert.ToSingle(n)/255f)).ToArray();

						string texture_page_number = readText[i + 2];		string t_page = "";

						foreach (var item in texture_page_list)
								if (item.Value == texture_page_number) t_page = item.Key.ToString();

						if (uv_ar.Length > 6) 
						{
								string[] tface4 = readText[i].Split(',', ' ');

								string face431 = tface4[0] + " " + tface4[1] + " " + tface4[2];
								string face432 = tface4[0] + " " + tface4[2] + " " + tface4[3];

								string uv_ar_816 = uv_ar[0] + " " + uv_ar[4] + " " + uv_ar[1] + " " 
								                 + uv_ar[5] + " " + uv_ar[2] + " " + uv_ar[6];

								string uv_ar_826 = uv_ar[0] + " " + uv_ar[4] + " " + uv_ar[2] + " " 
								                 + uv_ar[6] + " " + uv_ar[3] + " " + uv_ar[7];

								WritePLYList.Add(polySTR + face431 + " 6 " + uv_ar_816 + " " + t_page);
								WritePLYList.Add(polySTR + face432 + " 6 " + uv_ar_826 + " " + t_page);
						}

						else 
						{
								//FaceList[j] = readText[i].Replace(',', ' '); // не трогай. Будем добавлять в список 
								string[] tface3 = readText[i].Split(',', ' ');
								string uv_ar_6 = uv_ar[0] + " " + uv_ar[3] + " " + uv_ar[1] + " " + uv_ar[4] + " " + uv_ar[2] + " " + uv_ar[5];
								WritePLYList.Add(polySTR + string.Join(" ", tface3) + " 6 " + uv_ar_6 + " " + t_page);
						}
				}
		}

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

} // конец класса
