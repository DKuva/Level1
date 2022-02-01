Inventory Ststem -Dario Kuvacic

Keybinds -
WASD - movement
I - inventory
E - equipment
C - Attributes
Space - SpawnItem,  za promjenit item, pod player-playerscript promjenit "Item to spawn", itemi su u folder "Items"
F - ako je item "in air", droppat ga kraj playera

Relevantni kodovi po zadatku:
1. playerScript
2. lootableObject, playerScript
3. playerScript
4. playerInventory,inventorySlot
5. playerEquipment,equipSlot
6. playerInventory,inventorySlot
7. playerInventory,inventorySlot,playerEquipment,equipSlot,inAirItem,detailsPanel
8. item
9. playerAttributes


Kod_____

Sustav je izmisljen tako da "inventory" bude sami inventory panel, u smislu da postoji objekt koji uzima podatke iz nekog itemData, i samo
se instancira na panel. Hjerahija je takva da sav UI je child od Playera i kamere, tako se osigura da uvijek prati playera, 
pod Player/Camera/ui su Inventory, equipment, attribute, playerOverlay i par dodatnih panela.

U Inventory je PlayerInventory skripta, ispod inventory, u Inventory/.../Container/InventorySlot su inventorySlot skripte,
playerInventory ima array svih InventorySlotova, te funkcije addToInventory, i placeItemInSlot, prva trazi prvo slobodno mjesto u inventory i stavlja item tu,
druga uzima fiksni slot (x,y u inventory grid) i s pretpostavkom da je slobodan ga stavi u slot. 
Osim toga ima par funkcija sta su potrebne za druge djelove koda, i funkcija addRow koja dodaje jos jedan red u inventory,
svi InventorySlot objekti su instance, tako novi red se napravi tako da se samo instancira jos 8 Inventory slotova, i stari inventory array, zamjeni s novim.
To spada pod 3. tip inventory-a, no ima par buggova koji nisu popravljeni, scrollanje se rjesava sa unity na standardni nacin.
Za 4. tip bi trebalo napravit funkcije "isRowEmpty" koja kad je item maknut iz inventorya gleda ako igdje ima slobodan red, ako ima onda na slican
nacin kao addRow() generirat novi array i novih x*y instanca, no bez tog reda. No za taj dio nisam imao vremena.

InventorySlot ima glanve funkcije OnPointerDown, koja odlucuje sto se desava kad se klikne na item u inventory, addItemToSlot koja dodaje jedan item u slot,
i removeItemInSlot koja makne jedan item iz slota. addItemToSlot ce pokusati staviti item, tj, ako nema mijesta, stack je na max, itd, funkcija samo vraca false,
s tom funkcijom se tako provjerava dali ima mijesta na odredenom slot-u.
Ako Item nije 1x1 dimenzija u grid-u, funkcija ce deaktivirat susjedne slotove, i scaleat sebe na potrebnu dimenziju. playerInventory.addToInventory ignorira 
deaktivirane slotove kad trazi slobodni slot. Takoder kad inventorySlot provjerava dali ima mjesta, uzima u obzir dali ima AxB mjesta oko pritisnutog Slota.

Ako Item ima vise od jednog itema, tj, stack, moze se direkto mjenjat item Stack u slotu sa changeStackNumber, normalno addItemToSlot mjenja slot za 1.
playerInventory.addToInventory nema opciju da doda stack, za potrebu ovoga zadatka nije bilo potrebno, no moze se dodat tako da se zove changeStackSize,
tako se nemora zvat addItemToSlot vise puta, koji je puno tezi kod.

Equipment sadrzi playerEquipment koji je vrlo slican Inventory-u, samo pojednostavljen i malo promjenjen tako da nedozvoljava stavljanje u pogresni slot.
Isto ima EquipSlot koji je samo jednostavnija verzija InventorySlot.

Za prenosenje itema, tj "inAir" stanje, koristi se posebni panel "inAirObject". Kad Igrac klikne na InventorySlot, Ako je inAirObject aktivan, pokusa se stavit item u 
inventory, ako nije aktivan, InAirItem se aktivira, uzima item, tj, pamti ga, pamti stack, pamti koji je slot(tako da se moze vratit tu po potrebi), i InventorySlot se isprazni.
Isto vrijedi u slucaju sa equipSlot

Za opcije s desnim klikom, za brzo equippanje itd, samo se u OnPointerDown funkcijama pozivaju funkije u playerInventory/Equipment koje stavljaju/micu iteme,
ponovo, Inventory/Equip Slot su jedino mjesto gdje je taj item instanciran,tj sami item tehnicki nepostoji, samo instancirani slotovi uzimaju podatke iz itema i postavljaju sebe
kao odredeni item, tako za prijenos je samo dovoljno dodat ga u prvi slobodni slot na suprotnom panelu, i izbrisatiga na svom panelu. Netreba mjenjat vrijednosti u nikakvim sporednim listama ili drugo.

Attributes panel sadrzi playerAttributes koji se poziva iz equipSlot kad je dodan item, te se promjene i ispisu atributi u playerAttributes.

Osim navedenog u UI ima jos Message box koji po potrebi ispise poruku igracu te se automatski makne nakon X Update-a, details panel koji se aktivira kad se hovera
iznad inventoryItema, te ispise par podataka o itemu. i playerOverlay koji gleda ako je igrac kliknuo van aktivnih panela, i isto sadrzi botune za otvaranje panela.

Sami Item je ScriptableObject sa svim parametrima koje item moze imat. Za stvaranje novog itema, samo se kroz asset menu stvori novi item i ispune potrebna polja.
Nedostatak tog koda je sto ima samo jedan Item object, u smislu da kad se radi novi object svi parametri su podesivi, tako se moze napravit consumeable kaciga.
Osim toga, unity nezeli serialize-at dictionary, tako podaci o atributima nekog itema, npr +5 int za consumeable kacigu se unose kao array, sami
atributi tako nisu definirani na jednom mjestu, tako ako se treba dodat novi atribut, treba se podesit item i playerAttributes, sto je nezgodno,
i ako se krivo unese array za item moze doc do greske u izvodenju, sto je isto nezgodno. 
Alternativa je bila svoriti par child klasa od item, npr equippableItem, koji automatski ispunjuju neke vrijednosti u item, i staviti da se 
atributi ispunjuju u posebne public varijable koje na awake se stave u array, no kako se Item nikad ne instancira, nego samo doda u GameObject kao varijablu,
Awake se nikad ne zove. Tako radi manjka vremena, kod je ostao kakav je.

LootableObject je objekt koji igrac moze sakupit, to je samo jedan item, tako ako se baci stack itema, stvori se X LootableObjecta. Za ustedit vrijeme i za ove zadatke,
nije bilo potrebno radit objetke kao lootableChest itd.

PlayerScript osim kretanja rjesava sve ostale sitnice, otvaranje/zatvaranje prozora, skuplja iteme. U vecem projektu bi dao posebne objekte za neke stvari, no za ovo 
nije potrebno.

