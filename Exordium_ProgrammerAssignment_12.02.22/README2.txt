Inventory System - Level2 - Dario Kuvacic

Relevantni kodovi po zadatku:
1. cameraMovement, lookTrigger
2. lootableObject, PlayerScript
3. -
4. equipSlot, playerEquipment
5. detailsPanel
6. equipSlot, playerEquipment
7. playerAttributes
8. playerAttributes, Buff, Item, consumeableItem,equipableItem
9. playerMovementAndroid, inventorySlot,equipSlot,inAirItem
10. PlayerScript, playerMovement,playerInventory,playerEquipmment

------------
Kod
--------

Princip koda je ostao isti od prošlog zadatka, s tim da je promjenjeno nekoliko stvari. U skladu sa vašim feedbackom, i mojim komentarima, dodao sam nekoliko funkcija,
playerUI je primarna funkcija za otvaranje elemenata UIa, te sadrzi sve objekte u Ui, ona je komponenta od playera, tako umijesto da se koristi GameObject.Find, vecina toga se
nalazi preko player gameObjekta, koji se doda u komponentu u editoru. Također sam promjenio definiciju Itema, dodao posebne funkcije za svaki tip, te olakšao unos samog itema.

Što se tiće zadataka, pokreti s kamerom se vrše kroz cameraMovement, samo je dodana jednostavna funkcija koja miče transform u smjeru playera ovisno o nekoj brzini, funkcija 
na isti način može početi pratiti bilokiji point, te zadržat se na par sekundi.  
Drugi zadatak je samo malo prošireno od prvog djela, dodan je na par mjesta flag "actionButton", koji se togglea u funkcijama playerMovement i plazerMovementAndroid.
Dodat novi slot za item je isto jednostavno, samo se stvori nova instanca u hjerarhiji, ostalo je sve automatski više manje od prvog djela, samo je bilo potrebno definirat nove
tipove u item class. Isto vrijedi za sedmi zadatak, dodavanje novog atributa.

Split stack menu sam dodao na detailsPanel, nisam bio siguran kako ima smisla to staviti drugdje, jer ako se klikne na item, on prati miš i dosta je nezgodno s itemon pritiskati
druge botune. Tako se samo uz podatke itema, pojavi se dodatni prozor, ako item ima stack, gdje se može stack podjeliti. Stavio sam samo jedan botun "split" ,umjesto ok i cancel, jer
iskreno neznam što bi oni radili. Možda se ovo moje malo odmaklo od originalne ideje, al nadam se da je dovoljno.
Durability je dodan na equipableItem, u playerScript se broje koraci, te se poziva funkcija u playerEquipment koja troši svaki item s hodanjem.

Za 8. zadatak napravio sam posebnu klasu "Buff", i stavio da consumableItem može imati proizvoljni broj Buff klasa, koji se stave u Listu u playerAttributes kad se iskoristi consumableItem,
consumeableItem također može sam promjenit playerAttributes, ali to radi samo kao instantnu promjenu. Buff daje tražene tri opcije, i dodatnu opciju "permanent", gdje se označuje
da nakon što buff istekne, se attribut ne vraća u početno stanje. Sistem je fleksibilan, tako za dodatne parametre "health" i "mana", može ih se tretirat isto kao strength i ostale
atribute, samo je dodana funkcija koja postavlja njihove limite.

Za android build, ideja je bila da izbjegnem radit novi projekt i radit brdo novog koda, tako pokušao sam iskoristit funkcije OnPointerDown, i unity touch funkcije da napravim sve u istim kodovima,
i bilo je relativno uspješno. Promjenjene su funkcije inventorySlot, equipSlot, i inAirItem, tako da su malo fleksibilnije i urednije, te je samo dodan dio u onPointerDown gdje se kod
grana ovisno o build tipu. Jedina funkcija koja je posebna za android je playerMovementAndroid. Jedini plugin je Joystick, cisto da skratim na vremenu. 
Sama detekcija gestura je jošuvijek malo clunky, između mog starog mobitela i unity remote, nije bilo baš lako dobit dobar feeling za touch, no funkcjonalno je.
Idealno bi bilo koristit plugin za detekciju gestura, koji bi bio precizniji, no ponovo, to bi zahtjevalo mjenjanje dosta koda, te kako bi uštedio na vremenu, nisam išao tom rutom.

Dodatne napomene da Build verzije imaju par buggova, u editoru je tip-top, no u build verziji iskaču null errori, radi toga što paneli u Ui nisu prošli Awake() funkciju, u editoru,
prije pokretanja svi prozori su otvoreni, te na kraju Awake() se ugasi objekt, no tjekom buildanja nešto se očito zapetljalo. Isto tako, meni osobno, u android buildu nikako se ne 
detektira double tap, kao da Touch.touchCount uopće neradi. 
