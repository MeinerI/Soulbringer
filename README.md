# Soulbringer_game

**************************************************************************************************

map2ply.cs - текструры можно рипнуть NinjaRipper .

Разработчик dgVoodoo знает как точно они "зашифрованы" .

Я примерно представляю , но запрограммировать сейчас не смогу .

****************************************************************************************************
В программах присутствуют ошибки округления текстурных координат ,

поэтому текстуры накладываются чуть-чуть неправильно .

**************************************************************************************************

ВНИМАНИЕ ! Эти модели битые и при конвертировании выбрасывается исключение .

level\dune\thardtop.OB
level\mine\FACETR.OB
level\mine\HUT2.OB
level\mine\store.ob

**************************************************************************************************

ob2obj(with_duplication_of_vertices).cs - дублируются вершины и ув .

ob2obj(without_duplicating_vertices).cs - убраны дубли вершин .

**************************************************************************************************

ob2ply.cs - формат ply хранит вершины без дублирования ув и координат (?) .

Все модели должны находится в папке с ob2ply.exe .

**************************************************************************************************

ob2ply(search_in_subfolders).cs - (улучшенная версия) ищет

и конвертит модели прямо в папках своего уровня .

**************************************************************************************************

Импортируйте модели в Юнити с scale = 0,005 . Карты идут в масштабе 1:1

**************************************************************************************************

В файлах *.spt хранится инфа о положении вершин и виде блоков/колонн/стен/полов (см. в папке spt).

В файлах *.spg храняться uv этих блоков ?

**************************************************************************************************
