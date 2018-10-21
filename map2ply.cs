// ����� � ����������� *.map � *.hed ������ ��������� � ����� ����� � c��������������� *.exe
// �������� � ��������� ����� �� ���� - ����� ������ � Unity - ���� ��������� ����� ���������� ;)
// ����� ���� �������� � ��������� , �� ��� �� ������

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//�����������������������������������������������������������������������������������������������������

public class EarthGenerator
{

//�����������������������������������������������������������������������������������������������������

		static void Main()
		{

//�����������������������������������������������������������������������������������������������������

				List<string> WritePLYList = new List<string>(); // ���� ����� ��������� ��� ������ 
				List<string> faceList = new List<string>();

//�����������������������������������������������������������������������������������������������������

				List<float> blist_height = new List<float>() ;
				List<float> blist_scale  = new List<float>() ;

				List<byte>  block_type   = new List<byte>() ;

				List<byte>            texture   = new List<byte>() ;
				SortedList<int, byte> textureSL = new SortedList<int, byte>();

//�����������������������������������������������������������������������������������������������������

				// ����� ������ ������� // ���� ��� obj ��� �� ����� ����� �� // ��� � ���� ������ ��������
				System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

//�����������������������������������������������������������������������������������������������������

				DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

				FileInfo[] MapPath = d.GetFiles("*.map");

//�����������������������������������������������������������������������������������������������������

				foreach (FileInfo file in MapPath)
				{

//�����������������������������������������������������������������������������������������������������

						byte[] array1d  = File.ReadAllBytes(file.Name) ;

						string HedFileName = Path.GetFileNameWithoutExtension(file.Name) + ".hed";
						string[] readText = File.ReadAllLines(HedFileName);

						string width_str = readText[1]; // DIMENSIONS: WIDTH:32 HEIGHT:32

						string pattern = @"\d+";
						Regex r = new Regex(pattern);
						Match m = r.Match(width_str);

						//int[] width_and_height = width_str.Split(':',' ').Select(n => Convert.ToInt32(n)).ToArray(); 

						int width  = Convert.ToInt32(m.ToString());             //width_and_height[0]
						int height = Convert.ToInt32(m.NextMatch().ToString()); //width_and_height[1]

//�����������������������������������������������������������������������������������������������������

						List<string> uvs_quad = new List<string>();

						int j = 0 ; // ��� �������� textureSL �������

						for ( int i = 0 ; i < array1d.Length ; i += 56 )
						{
							// vertex.y

							blist_height.Add ( array1d [i + 36] / 255f ) ; // ������ 
							blist_scale.Add  ( array1d [i + 37] /   1f ) ; // �������

							// texture

							texture.Add ( array1d[i + 32] ) ;
							if (!textureSL.ContainsValue(array1d[i + 32]))
									 textureSL.Add ( j++ , array1d[i + 32] ) ;

							// type of block

							block_type.Add (array1d[i + 44]) ;	// ��� �����

							// uvs

							uvs_quad.Add ("6 " 
													+ array1d[i +  2]/255f + " " + array1d[i + 18]/255f + " " 
													+ array1d[i +  6]/255f + " " + array1d[i + 22]/255f + " " 
													+ array1d[i + 14]/255f + " " + array1d[i + 30]/255f ) ;
							uvs_quad.Add ( "6 " 
													+ array1d[i +  6]/255f + " " + array1d[i + 22]/255f + " " 
													+ array1d[i + 10]/255f + " " + array1d[i + 26]/255f + " " 
													+ array1d[i + 14]/255f + " " + array1d[i + 30]/255f ) ;
						}
/*
						Console.WriteLine ( "height "     + blist_height[0] ) ;
						Console.WriteLine ( "scale "      + blist_scale[0]  ) ;
						Console.WriteLine ( "textureSL "  + textureSL[0]    ) ;
						Console.WriteLine ( "uvs_quad "   + uvs_quad[0]     ) ;
						Console.WriteLine ( "block_type " + block_type[0]   ) ;
*/
						//Console.WriteLine("������ �������") ;
						//foreach ( var item in textureSL ) Console.WriteLine(item) ;

						//Console.WriteLine("������ ���� uvs_per_dace") ;
						//for ( int i = 0 ; i < 8 ; i++ )	Console.WriteLine(uvs_quad[i]*255f);

//�����������������������������������������������������������������������������������������������������
//�����������������������������������������������������������������������������������������������������
//�����������������������������������������������������������������������������������������������������

						WritePLYList.Add("ply");	//	���������� ����� 

						WritePLYList.Add("format ascii 1.0");	//	������������ ������ �������

						foreach (var item in textureSL) // ��������� ��� ������ ���������� � ������ - � ���� ������������ � ����
						{
							WritePLYList.Add("comment TextureFile " 
							// + file.Name + "_" 
							+ item.Value + ".png") ;
						}

						WritePLYList.Add("element vertex " + (width*height) ) ; // ���������� ������

						WritePLYList.Add("property float x") ;
						WritePLYList.Add("property float y") ;
						WritePLYList.Add("property float z") ;

//�����������������������������������������������������������������������������������������������������

						for ( int x = 0 ; x < (width*height) ; x++ )
						{
								faceList.Add(x + " " +     (++x) + " " + (x+width-1)) ;  //  0  1 32   991  992 1023
								faceList.Add(x + " " + (x+width) + " " + (x+width-1)) ;  //  1 33 32   992 1024 1023
								x-- ;
								if ( ( x + 2 ) % ( width ) == 0 ) x++ ;
								if ( x >  ( ( width * height ) - width - 2 ) ) break;
						}

						int face_counter = faceList.Count; faceList.Clear(); 
/*
						if ( face_counter == 16257 ) face_counter = 16065 ;
						if ( face_counter == 22017 ) face_counter = 21971 ;
						if ( face_counter == 32513 ) face_counter = 32385 ; // 32766
						if ( face_counter == 1922  ) face_counter = 1920  ;
*/
						WritePLYList.Add("element face " + face_counter); // ����� ���������� ����������� ������

//�����������������������������������������������������������������������������������������������������

						WritePLYList.Add("property list uchar int vertex_indices"); 

						WritePLYList.Add("property list uchar float texcoord");
						WritePLYList.Add("property int texnumber");

						WritePLYList.Add("end_header") ; // ����� ���������

//�����������������������������������������������������������������������������������������������������

						// ��������� � PLY ������ �� (width*height) ������ � ���� ��������� X Y Z float-����

						int y = 0 ;

						for ( int x = 0 ; x < height ; x++ )
						{
								for ( int z = 0 ; z < width ; z++ )
								{
										float yy = - ( blist_height[y] + blist_scale[y] ) ;
										WritePLYList.Add((-x) + " " + yy + " " + (z)) ;
										y++ ;
								}
						}

//�����������������������������������������������������������������������������������������������������

						// ��������� � PLY ������ ������ � ���� [	3 v1 v2 v3 6 x1 y1 x2 y2 x3 y3 texture_page	]

						int t_page = 0 ;

						for ( int x = 0 , uv_t = 0 , i = 0 ; x < (width*height) ; x++ , uv_t++ , i++ )
						{
								//�����������������������������������������������������������������������������������������������������

									foreach (var item in textureSL)
											if (item.Value == texture[i]) 
													t_page = item.Key;

								//�����������������������������������������������������������������������������������������������������

//							int v1 = x ; int v2 = x + 1 ; int v3 = x + width ; int v4 = x + width - 1 ;

								WritePLYList.Add ( "3 " + x + " " +     (++x) + " " + (x+width-1) + " " + uvs_quad[uv_t] + " " + t_page);  //  0  1 32___0 / 1___990  991 1022

								uv_t++; // � ������� ���� - ����������� ������ �� �� ������ ����������� , � ��������� ������� - �� ������ 

								WritePLYList.Add ( "3 " + x + " " + (x+width) + " " + (x+width-1) + " " + uvs_quad[uv_t] + " " + t_page);  //  1 33 32___32/33___991 1023 1022

								x-- ; // "�����" ������� ������ 
 
								if ( ( x + 2 ) % ( width ) == 0 ) x++ ; // ���� 31+1 % 32 == 0

								if ( ( i + 2 ) % ( width ) == 0 ) i++ ; // ���� ��� �������� ������ ������� �� ������ ������ , �� ��������� �� �������� ������ , ����������� �������

//							if ( x > ( ( width * height ) - width - 2 ) ) break ;

								//�����������������������������������������������������������������������������������������������������

								//  60  61 ( 62 -  63)  64  65
								// 124 125 (126 - 127) 128 129
								// 189 190 (191 - 192) 193 194

//							if ( uv_t > 2045 ) break;

								if ( 
								   (   ( uv_t + 3 ) % ( width ) == 0 )
								   &&
								   ( ( ( uv_t + 3 ) / ( width ) ) % 2 == 0 )
								   )
								{
//									Console.WriteLine ( uv_t ) ; 
										uv_t+=2 ; 
								}

								//      61+3 =64%32=0
								//     125+3=128%32=0
								//     189+3=192%32=0

						}

//�����������������������������������������������������������������������������������������������������

						// ������ PLY ����

						string FileName = Path.GetFileNameWithoutExtension(file.Name); // "��� ����� ��� ����������"
						string WriteFilePath = FileName + "___PLY.ply" ; // "���[Map]�����(���[.map])___PLY.ply"
						string[] PLY_write_Text = WritePLYList.ToArray() ;  // ��� ������ �� ������ � ������ �����
						File.WriteAllLines(WriteFilePath, PLY_write_Text) ; // ������ ����� ���������� � ��������� ����

//�����������������������������������������������������������������������������������������������������
/*
						Console.WriteLine ( "���������� ������  = " + blist_scale.Count ) ;
						Console.WriteLine ( "���������� ������  = " + (width*height)    ) ;
						Console.WriteLine ( "���������� ������� = " + texture.Count     ) ;
						Console.WriteLine ( "���������� face    = " + face_counter      ) ;
						Console.WriteLine ( "���������� uvs     = " + uvs_quad.Count    ) ;
						Console.WriteLine ( "" ) ;
*/
//�����������������������������������������������������������������������������������������������������

						WritePLYList.Clear();
						blist_height.Clear();
						blist_scale.Clear();
						block_type.Clear();
						uvs_quad.Clear();
						faceList.Clear();
						textureSL.Clear();
						texture.Clear();

//�����������������������������������������������������������������������������������������������������

//					Console.WriteLine ( file.Name + " \t OK") ;

//�����������������������������������������������������������������������������������������������������

				} // foreach

//�����������������������������������������������������������������������������������������������������

		} // Main

//�����������������������������������������������������������������������������������������������������

} // class

//�����������������������������������������������������������������������������������������������������
