// файлы с расширением *.ob должны находится в одной папке с cкомпилированным *.exe

// в *.obj-файлах отсутствует дублирование вершин , но осталось uv

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

		static string[] readText; // хранит все строки из файла
		static int VTXS, POLY4, POLY3; // хранят количество вершин и т.д.

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

//	сюда будем добавлять все пары vt u v

//	Представляет коллекцию пар «ключ-значение», упорядоченных по ключу.
//	static SortedDictionary<int, string> vt_uv = new SortedDictionary<int, string>(); 

//	Представляет коллекцию пар «ключ-значение», упорядоченных по ключу на основе реализации IComparer<T>.
		static SortedList<int, string> vt_uv = new SortedList<int, string>();

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

		static List<string> WriteObjList = new List<string>(); // сюда будем добавлять все строки 
		static List<string> FaceList = new List<string>();
		static List<string> VertexList = new List<string>();

		static HashSet<string> newmtl = new HashSet<string>();	// для *.mtl файла
		static List<string>		 usemtl = new List<string>();

///////////////////////////////////////////////////////////////////////////////////////////////////////////////

		static int iii = 1 ; // "счётчик" "совпадений" вершинных индексов
		static int vii;
		static string[] VertexArray;	//	хранит все строки с вершинами (для дублирования)

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

    public static int Main(string[] args)
    {
				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// точки вместо запятых				// хотя для obj это не важно вроде бы				// как и табы вместо пробелов
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// берём информацию о текущем каталоге (включая имена файлов)
				DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

				// берем инфу о всех *.ob файлах в текущем каталоге
				FileInfo[] Files = d.GetFiles("*.ob");
				
				///////////////////////////////////////////////////////////////////////////////////////////////////////////////
				
				foreach (FileInfo file in Files) // для каждого файла
				{
						// читаем все строки в массив
						readText = File.ReadAllLines(file.Name);

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////

						VTXS  = int.Parse(readText[2].Split(' ')[1]);	//	вершины
						POLY4 = int.Parse(readText[4].Split(' ')[1]);	//	квадраты
						POLY3 = int.Parse(readText[5].Split(' ')[1]);	//	треугольники
						
						VertexArray = new string[VTXS];

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////

// Готовим файл на будушее - к обратной конвертации из *.obj в *.ob 
// Возможно это бесполезно, потому что некоторые редакторы - изменяют порядок строк при импорте/экспорте

#if OB2OBJ
						WriteObjList.Add("#" + readText[0]);
#endif
//					string mtlFileName = "mtllib " + file.Name + ".mtl" + Environment.NewLine; 
						string mtlFileName = "mtllib " + Path.GetFileNameWithoutExtension(file.Name) + "_mtl.mtl" + Environment.NewLine; 
						WriteObjList.Add(mtlFileName); // добавим строку, содержащюю инфу о подключаемой библиотеке материалов
#if OB2OBJ
						// переписываем первые десять строк в список для нового файла
						for ( int s = 1 ; s < 10 ; s++ ) WriteObjList.Add( "#" + readText[s] );
#endif
						/////////////////////
						// ВЕРШИНЫ v x y z /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////

						int VTXS10 = 10; // координаты вершин в файле *.ob - всегда начинаются с 11-ой строки = 10 индекс в массиве

						for ( int vi = 0 ; vi < VTXS ; vi++ )		// читать VTXS строк массива		//		x,y,z
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
								VertexList.Add( VertexArray[vi] ); // добавить в массив - обработанные строки "первых/оригинальных" вершин
						}

						// WriteObjList.Add(""); // строка между "первыми" и "вторыми" вершинами

						////////////////////////
						// f 1 2 3 (4) //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						////////////////////////

						usemtl.Add("0"); usemtl.Add("0"); // это чтобы не повторялись строки usemtl № // как то работает :D

						             int POLY_index = 0; // начиная с этой (строки/элемента массива строк) проходим по всем строкам с фейсами
						if (POLY4 > 0) { POLY_index = VTXS10 + VTXS + 7 ;                   Method(4, POLY_index); }
						if (POLY3 > 0) { POLY_index = VTXS10 + VTXS + 6 + (POLY4 * 5) + 3 ; Method(3, POLY_index); }

						/////////////////////
						// ВЕРШИНЫ v x y z /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////

						foreach (var vert in VertexList)	// добавляем дубликаты Вершин в список WriteObjList 
						{
								WriteObjList.Add(vert);  // они получены после "дублироавния" UV "из-за" фейсов
						}

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////
#if OB2OBJ
						WriteObjList.Add(""); // добавить линию после "массива" вершин
						WriteObjList.Add("#OBJS");
						WriteObjList.Add(""); 
						WriteObjList.Add("#" + readText [ VTXS10 + VTXS + 3 ] ); 
#endif
						WriteObjList.Add(""); // добавить линию после OBJS v,f3,f4,0,0,0,0,0

						///////////////////////////////////
						// ТЕКСТУРНЫЕ КООРДИНАТЫ vt u v //////////////////////////////////////////////////////////////////////////////////
						//////////////////////////////////

						// WriteObjList.Add(""); // добавляет линию после вершин

						foreach (var item in vt_uv) 	    // добавляет uv в список WriteObjList
						{
							//количество УВ в некоторых файлах, почему то меньше чем вершин
								WriteObjList.Add(item.Value);	// упорядоченные по индексу по возрастанию строки вида vt_u_v
							//Console.WriteLine(item.Value);
						}

						WriteObjList.Add(""); 

						////////////////////////
						// f 1 2 3 (4) //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						////////////////////////

						foreach (var face in FaceList)	// добавляем faces в список WriteObjList
						{
								WriteObjList.Add(face);
						}

#if OB2OBJ
						WriteObjList.Add(""); 
						WriteObjList.Add("#FACE4"); 
						WriteObjList.Add(""); 
#endif
						/////////////////////////////
						// СОЗДАЁМ ФАЙЛЫ obj и mtl ////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////////////

						string WriteFilePath = Path.GetFileNameWithoutExtension(file.Name) + "_obj.obj";
						string[] OBJ_write_Text = WriteObjList.ToArray();
						File.WriteAllLines(WriteFilePath, OBJ_write_Text);

						//----------------------------------------------------------------------------------------------------------------------------------------------------

						List<string> mtlList = new List<string>(); 

						foreach ( string nmat in newmtl )
						{
								mtlList.Add("newmtl " + nmat);           //	добавляем строку newmtl ##
								mtlList.Add("map_Kd " + nmat + ".png");  //	добавляем строку map_Kd ##.dds
								mtlList.Add("");
						}

						string[] MTL_write_Text = mtlList.ToArray();
						string WriteMTLPath = Path.GetFileNameWithoutExtension(file.Name) + "_mtl.mtl";
						File.WriteAllLines(WriteMTLPath, MTL_write_Text);

						//----------------------------------------------------------------------------------------------------------------------------------------------------

						// очищаем все коллекции, а то они накапливаются 

						VertexList.Clear();	
						vt_uv.Clear();	
						FaceList.Clear(); 
						newmtl.Clear();	
						mtlList.Clear();	
						usemtl.Clear(); 
						WriteObjList.Clear(); 

						iii = 1; // для новых файлов - очищаем счётчик новых вершин

						//---------------------------------------------------------------------------------------------------------------------------------------
				}

				if ( Files.Length > 0 ) Console.WriteLine("Processed {0} file(s)", Files.Length);
				else Console.WriteLine("В выбранном каталоге - нет файлов с моделями!");

				return 0;
		} 

// конец Main()//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

		private static void Method(int poly_number, int POLYindex)
		{
				string[] POLYwords = new string[poly_number]; // 3 или 4 строки Индексов Вершин

				                    int POLY = 0; string polySTR = ""; // =_=
				if (poly_number == 4) { POLY = POLY4;    polySTR = "#POLY4"; } 
				if (poly_number == 3) { POLY = POLY3;    polySTR = "#POLY3"; } 

#if OB2OBJ
				FaceList.Add(polySTR); 
				FaceList.Add(""); 
#endif
				for ( int i = POLYindex ; i < POLYindex + POLY * 5; i+=5 ) // каждую строку с 1,2,3(,4)
				{
						// берём [i] строку фейсов, выбираем значения n , которые конвертятся в Int32 и помещаем их в массив ?

						int[] POLYface = readText[i].Split(',' , ' ').Select(n => Convert.ToInt32(n) + 1 ).ToArray();	// (0)+1 (1)+1 (2)+1 ((3)+1)

						for ( int k = 0 ; k < POLYface.Length ; k++ ) 
						{
								POLYwords[k] = POLYface[k] + "/" + POLYface[k];  // 1/1 2/2 3/3 (4/4)

								// берём следущую строку // делаем примерно тоже самое
								int[] POLYuvs = readText[i + 1].Split(',').Select(uvs => Convert.ToInt32(uvs)).ToArray();	//	1 2 3 4 5 6 (7 8)

								//double u = Math.Round((double)POLYuvs[k]/255f, 2);									// 1 2 3 // 1 2 3 4
								float u = POLYuvs[k]/255f;
								
								//double v = Math.Round((double)POLYuvs[k + poly_number]/255f, 2);		// 4 5 6 // 5 6 7 8
								float v = POLYuvs[k + poly_number]/255f;

								string vtuv = "vt" + "\t" + u + "\t" + v; // vt 1 4 // vt 1 5 

								if ( !vt_uv.ContainsKey(POLYface[k]) ) // если ключа 1 нет, то присваиваем ему значение
								{
									vt_uv[POLYface[k]] = vtuv; 
#if OB2OBJ
									Console.WriteLine( POLYface[k] + "\t" + "\t" + vt_uv[POLYface[k]] ); 
#endif
								}
								else // иначе, если ключ есть, то создаём новый ключ и присваиваем ему "новое" +1 значение
								{
									vii = VTXS + iii++ ;
									vt_uv.Add( vii , vtuv );	// добавляем новую пару (и индекс) текстурных координат
									POLYwords[k] = POLYface[k] + "/" + vii;	//	новая вершина (дубликат)/новые uv
									// VertexList.Add(VertexArray[POLYface[k]-1]); // добавляем дубликат вершины, -1 это потому что выше мы увеличили face на единицу
#if OB2OBJ
									Console.WriteLine( POLYface[k] + "\t" + vii + "\t" + vt_uv[POLYface[k]] ); // старые новые значения пар ut_uv
									//Console.WriteLine( vii + "\t" + "\t" + vii ); // новые пары вершин/координат
#endif
								}
						}

//----------------------------------------------------------------------------------------------------------------------------------------------------

						string POLYmat     = readText[i+2];		// usemtl № in *.obj
						string render_side = readText[i+3];

						//FaceList.Add("usemtl " + POLYmat); // раньше было так // просто добавлялась одинаковая строка

// usemtl //сделать чтобы не повторялись // готово

						usemtl.Add(POLYmat);	// добавляем POLYmat значение
						if (POLYmat != usemtl[usemtl.Count - 2]) // если не равно предыдущему, то пишем
								FaceList.Add("usemtl " + POLYmat);

////////////
						if (!newmtl.Contains(POLYmat)) newmtl.Add(POLYmat);    // newmtl № in *.mtl
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

//					WriteObjList.Add(""); // добавить линию после "массива" индексов

		} // конец метода Method(int poly_number, int POLYindex)

//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж
//жжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжжж

} // конец класса
