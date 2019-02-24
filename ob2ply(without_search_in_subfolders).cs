// ôàéëû ñ ðàñøèðåíèåì *.ob äîëæíû íàõîäèòñÿ â îäíîé ïàïêå ñ cêîìïèëèðîâàííûì *.exe

//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

sealed class Test
{

//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

		static List<string> WritePLYList = new List<string>(); // ñþäà áóäåì äîáàâëÿòü âñå ñòðîêè 
		static string[] readText; // õðàíèò âñå ñòðîêè èç ôàéëà
		static int VTXS, POLY4, POLY3; // õðàíÿò êîëè÷åñòâî âåðøèí è ò.ä.
		static string[] uv; // 6 0,09163 0,565323 0,109197 0,565733 0,10888 0,602539
		static string[] FaceList;
		static float[] uv_ar;

		static SortedList<int, string> texture_page_list = new SortedList<int, string>();

//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ
//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ
//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

    public static int Main(string[] args)
    {
    		int VTXS10 = 10; // êîîðäèíàòû âåðøèí â ôàéëå *.ob - âñåãäà íà÷èíàþòñÿ ñ 11-îé ñòðîêè = 10 èíäåêñ â ìàññèâå

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				// òî÷êè âìåñòî çàïÿòûõ				// õîòÿ äëÿ obj ýòî íå âàæíî âðîäå áû				// êàê è òàáû âìåñòî ïðîáåëîâ
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

				///////////////////////////////////////////////////////////////////////////////////////////////////////////////

				DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

				FileInfo[] Files = d.GetFiles("*.ob");
				
				foreach (FileInfo file in Files)
				{
						readText = File.ReadAllLines(file.Name);			// ÷èòàåì âñå ñòðîêè èç ôàéëà

						///////////////////////////////////////////////////////////////////////////////////////////////////////////////

						VTXS  = int.Parse(readText[2].Split(' ')[1]);	//	âåðøèíû
						POLY4 = int.Parse(readText[4].Split(' ')[1]);	//	êâàäðàòû
						POLY3 = int.Parse(readText[5].Split(' ')[1]);	//	òðåóãîëüíèêè

						string[] VertexArray = new string[VTXS]; // êîëè÷åñòâî ñòðîê ñ âåðøèíàìè

						//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

						// äîáàâèòü êîììåíòû ñ èìåíàìè òåêñòóð

						int POLY = 0; int POLYindex = 0; int j=0; // íà÷èíàÿ ñ ýòîé (ñòðîêè/ýëåìåíòà ìàññèâà ñòðîê) ïðîõîäèì ïî âñåì ñòðîêàì ñ ôåéñàìè

						if (POLY4 > 0) { POLYindex = VTXS10 + VTXS + 7 ; POLY = POLY4 ; }

						for ( int i = POLYindex ; i < POLYindex + POLY * 5; i+=5 ) // êàæäóþ ñòðîêó ñ íîìåðîì òåêñòóðû
						{
							if (!texture_page_list.ContainsValue(readText[i + 2]))
							{
										texture_page_list.Add ( j , readText[i + 2] ) ; 
										j++;
							}
						}

						// if(hashSet.Add(item)) orderList.Add(item);

						if (POLY3 > 0) { POLYindex = VTXS10 + VTXS + 6 + (POLY4 * 5) + 3 ; POLY = POLY3 ; }

						for ( int i = POLYindex ; i < POLYindex + POLY * 5; i+=5 ) // êàæäóþ ñòðîêó ñ íîìåðîì òåêñòóðû
						{
							if (!texture_page_list.ContainsValue(readText[i + 2]))
							{
										texture_page_list.Add ( j , readText[i + 2] ) ; 
										j++;
							}
						}

						//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

						WritePLYList.Add("ply");	//	ìàãè÷åñêîå ñëîâî 

						WritePLYList.Add("format ascii 1.0");	//	åäèíñòâåííàÿ âåðñèÿ ôîðìàòà

						//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

						foreach (var item in texture_page_list)
						{
							WritePLYList.Add("comment TextureFile " 
							// + file.Name + "_" 
							+ item.Value + ".png");
						}

						//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

//					WritePLYList.Add("");

						WritePLYList.Add("element vertex " + VTXS);	//	êîëè÷åñòâî âåðøèí

						WritePLYList.Add("property float x");
						WritePLYList.Add("property float y");
						WritePLYList.Add("property float z");

//					WritePLYList.Add("");

						WritePLYList.Add("element face " + ( POLY4*2 + POLY3 )); // îáùåå êîëè÷åñòâî òðåóãîëüíûõ ãðàíåé

						WritePLYList.Add("property list uchar int vertex_indices"); 
						// "vertex_indices" is a list of ints (ãäå uchar - èõ êîëè÷åñòâî? , à int - ðàçìåð çíà÷åíèé?)

					//WritePLYList.Add("property list ushort float texcoord");
						WritePLYList.Add("property list uchar float texcoord");
						WritePLYList.Add("property int texnumber");

						WritePLYList.Add("end_header"); // êîíåö çàãîëîâêà
/*
						WritePLYList.Add("");
						WritePLYList.Add("comment start of vertex list");	//	êîììåíò - íà÷àëî ñïèñêà âåðøèí
						WritePLYList.Add("");
*/
						/////////////////////
						// ÂÅÐØÈÍÛ v x y z /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////

//					WritePLYList.Add("");

						for ( int vi = 0 ; vi < VTXS ; vi++ )		// ÷èòàòü VTXS ñòðîê ìàññèâà		//		x,y,z
						{
								int[] vrtx_ar = readText[VTXS10 + vi].Split(',').Select(n => Convert.ToInt32(n)).ToArray();		//	x y z

								//VertexArray[vi] = ((-1)*vrtx_ar[1]).ToString() + " " + vrtx_ar[2].ToString() + " " + ((-1)*vrtx_ar[0]).ToString(); // -y z -x
								//VertexArray[vi] = ((-1)*vrtx_ar[0]).ToString() + " " + vrtx_ar[2].ToString() + " " + vrtx_ar[1].ToString();	//	-x y z

/*ýòî ðàáîòàåò
								VertexArray[vi] = Math.Round(-1*vrtx_ar[0]*0.005f , 2).ToString() + " " 
								                + Math.Round((double)vrtx_ar[2]*0.005f , 2).ToString() + " " 
								                + Math.Round((double)vrtx_ar[1]*0.005f , 2).ToString();	//	-x z y
*/
// à ýòî áåç îêðóãëåíèÿ

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

						int POLY_index = 0; // íà÷èíàÿ ñ ýòîé (ñòðîêè/ýëåìåíòà ìàññèâà ñòðîê) ïðîõîäèì ïî âñåì ñòðîêàì ñ ôåéñàìè

						if (POLY4 > 0) { POLY_index = VTXS10 + VTXS + 7 ; Method(4, POLY_index); /*WritePLYList.Add("");*/ } 

						if (POLY3 > 0) { POLY_index = VTXS10 + VTXS + 6 + (POLY4 * 5) + 3 ; Method(3, POLY_index); }

//					WritePLYList.Add("");

						/////////////////////////////
						// ÑÎÇÄÀ¨Ì ÔÀÉËÛ obj è mtl ////////////////////////////////////////////////////////////////////////////////////////
						/////////////////////////////

						string WriteFilePath = Path.GetFileNameWithoutExtension(file.Name) + "_ply.ply";
						string[] OBJ_write_Text = WritePLYList.ToArray();
						File.WriteAllLines(WriteFilePath, OBJ_write_Text);

						//----------------------------------------------------------------------------------------------------------------------------------------

						// î÷èùàåì âñå êîëëåêöèè, à òî îíè íàêàïëèâàþòñÿ 

						WritePLYList.Clear(); 
						//
						texture_page_list.Clear();

						//----------------------------------------------------------------------------------------------------------------------------------------
				}

				if ( Files.Length > 0 ) Console.WriteLine("Processed {0} file(s)", Files.Length);
				else Console.WriteLine("Â âûáðàííîì êàòàëîãå - íåò ôàéëîâ ñ ìîäåëÿìè!");

				return 0;
		}

//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ
//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ
//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

		private static void Method(int poly_number, int POLYindex)
		{
				                    int POLY = 0; string polySTR = ""; // =_=
				if (poly_number == 3) { POLY = POLY3;    polySTR = "3 "; } 
				if (poly_number == 4) { POLY = POLY4;    polySTR = "3 "; } 

				for ( int i = POLYindex ; i < POLYindex + POLY * 5 ; i+=5 ) // êàæäóþ ñòðîêó ñ 1,2,3(,4)
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
								//FaceList[j] = readText[i].Replace(',', ' '); // íå òðîãàé. Áóäåì äîáàâëÿòü â ñïèñîê 
								string[] tface3 = readText[i].Split(',', ' ');
								string uv_ar_6 = uv_ar[0] + " " + uv_ar[3] + " " + uv_ar[1] + " " + uv_ar[4] + " " + uv_ar[2] + " " + uv_ar[5];
								WritePLYList.Add(polySTR + string.Join(" ", tface3) + " 6 " + uv_ar_6 + " " + t_page);
						}
				}
		}

//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ
//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ
//ææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææææ

} // êîíåö êëàññà
