//depois deletar

/* using UnityEngine;
 using UnityEditor;
 using UnityEditor.AnimatedValues;
 
 [CustomEditor(typeof(Item))]
 public class ItemEditor : Editor{

     AnimBool consumivel = new AnimBool(false);
     AnimBool chave = new AnimBool(false);
     AnimBool ferramenta = new AnimBool(false);
     AnimBool arma = new AnimBool(false);
     AnimBool outro = new AnimBool(false);
     //
     AnimBool ferramentaEscudo = new AnimBool(false);
     AnimBool ferramentaArco = new AnimBool(false);
     public override void OnInspectorGUI(){
         base.OnInspectorGUI();

         Item meuItem = (Item)target;
         switch(meuItem.tipoItem){
             case Item.tiposDeItem.consumivel:
                 consumivel.target = true;
                 chave.target = false;
                 ferramenta.target = false;
                 arma.target = false;
                 outro.target = false;
                 break;
             case Item.tiposDeItem.chave:
                 consumivel.target = false;
                 chave.target = true;
                 ferramenta.target = false;
                 arma.target = false;
                 outro.target = false;
                 break;
             case Item.tiposDeItem.ferramenta:
                 consumivel.target = false;
                 chave.target = false;
                 ferramenta.target = true;
                 arma.target = false;
                 outro.target = false;
                 break;
             case Item.tiposDeItem.arma:
                 consumivel.target = false;
                 chave.target = false;
                 ferramenta.target = false;
                 arma.target = true;
                 outro.target = false;
                 break;
             case Item.tiposDeItem.outro:
                 consumivel.target = false;
                 chave.target = false;
                 ferramenta.target = false;
                 arma.target = false;
                 outro.target = true;
                 break;
             default:
                 break;
         }

         if(ferramenta.target){
             switch(meuItem.tipoFerramenta){
                 case Item.tiposDeFerramenta.escudo:
                     ferramentaEscudo.target = true;
                     ferramentaArco.target = false;
                     break;
                 case Item.tiposDeFerramenta.arco:
                     ferramentaEscudo.target = false;
                     ferramentaArco.target = true;
                     break;
                 case Item.tiposDeFerramenta.gancho:
                     ferramentaEscudo.target = false;
                     ferramentaArco.target = false;
                     break;
                default:
                     break;
             }
         }

         // Estilo dos Headers
         GUIStyle estiloHeader = new GUIStyle();
         estiloHeader.fontSize = 13;
         estiloHeader.fontStyle = FontStyle.Bold;

         //Consumivel
         if(EditorGUILayout.BeginFadeGroup(consumivel.faded)){
             // meuItem.resistencia = EditorGUILayout.FloatField("Resistencia", meuItem.resistencia);
             // meuItem.efeito = EditorGUILayout.CommandEvent("Efeito");
         }
         else{
             // meuItem.resistencia = 0;
         }
         EditorGUILayout.EndFadeGroup();
         //ItemChave
         if(EditorGUILayout.BeginFadeGroup(chave.faded)){
             meuItem.nomeChave = EditorGUILayout.TextField("Nome Chave - ID", meuItem.nomeChave);
         }
         // else{
         //     meuItem.nomeChave = "";
         // }
         EditorGUILayout.EndFadeGroup();
         //Ferramenta
         if(EditorGUILayout.BeginFadeGroup(ferramenta.faded)){
             EditorGUILayout.LabelField("Tipo de Ferramenta", estiloHeader);
             meuItem.tipoFerramenta = (Item.tiposDeFerramenta)EditorGUILayout.EnumPopup("Tipo Ferramenta", meuItem.tipoFerramenta);
             //
             if(EditorGUILayout.BeginFadeGroup(ferramentaEscudo.faded)){
                 EditorGUILayout.LabelField("Defesa", estiloHeader);
                 meuItem.physicDefense = EditorGUILayout.FloatField("Physic Defense", meuItem.physicDefense);
                 meuItem.thunderDefense = EditorGUILayout.FloatField("Thunder Defense", meuItem.thunderDefense);
                 meuItem.voidDefense = EditorGUILayout.FloatField("Void Defense", meuItem.voidDefense);
                 meuItem.fireDefense = EditorGUILayout.FloatField("Fire Defense", meuItem.fireDefense);
                 meuItem.iceDefense = EditorGUILayout.FloatField("Ice Defense", meuItem.iceDefense);
             }
             EditorGUILayout.EndFadeGroup();
             //
             if(EditorGUILayout.BeginFadeGroup(ferramentaArco.faded)){
                 EditorGUILayout.LabelField("Velocidade", estiloHeader);
                 meuItem.velocidade = EditorGUILayout.FloatField("Velocidade", meuItem.velocidade);
                 EditorGUILayout.LabelField("Dano", estiloHeader);
                 meuItem.physicDamage = EditorGUILayout.FloatField("Physics Damage", meuItem.physicDamage);
                 meuItem.thunderDamage = EditorGUILayout.FloatField("Thunder Damage", meuItem.thunderDamage);
                 meuItem.voidDamage = EditorGUILayout.FloatField("Void Damage", meuItem.voidDamage);
                 meuItem.fireDamage = EditorGUILayout.FloatField("Fire Damage", meuItem.fireDamage);
                 meuItem.iceDamage = EditorGUILayout.FloatField("Ice Damage", meuItem.iceDamage);
             }
             EditorGUILayout.EndFadeGroup();
         }
         // else{
         //     meuItem.tipoFerramenta = Item.tiposDeFerramenta.escudo;
         // }
         EditorGUILayout.EndFadeGroup();
         //Arma
         if(EditorGUILayout.BeginFadeGroup(arma.faded)){
             //
             EditorGUILayout.LabelField("Tipo De Arma", estiloHeader);
             meuItem.tipoArma = (Item.tiposDeArma)EditorGUILayout.EnumPopup("Tipo Arma", meuItem.tipoArma);
             //
             EditorGUILayout.LabelField("Velocidade", estiloHeader);
             meuItem.velocidade = EditorGUILayout.FloatField("Velocidade", meuItem.velocidade);
             //
             EditorGUILayout.LabelField("Dano", estiloHeader);
             meuItem.physicDamage = EditorGUILayout.FloatField("Physics Damage", meuItem.physicDamage);
             meuItem.thunderDamage = EditorGUILayout.FloatField("Thunder Damage", meuItem.thunderDamage);
             meuItem.voidDamage = EditorGUILayout.FloatField("Void Damage", meuItem.voidDamage);
             meuItem.fireDamage = EditorGUILayout.FloatField("Fire Damage", meuItem.fireDamage);
             meuItem.iceDamage = EditorGUILayout.FloatField("Ice Damage", meuItem.iceDamage);
             //
             EditorGUILayout.LabelField("Defesa", estiloHeader);
             meuItem.physicDefense = EditorGUILayout.FloatField("Physic Defense", meuItem.physicDefense);
             meuItem.thunderDefense = EditorGUILayout.FloatField("Thunder Defense", meuItem.thunderDefense);
             meuItem.voidDefense = EditorGUILayout.FloatField("Void Defense", meuItem.voidDefense);
             meuItem.fireDefense = EditorGUILayout.FloatField("Fire Defense", meuItem.fireDefense);
             meuItem.iceDefense = EditorGUILayout.FloatField("Ice Defense", meuItem.iceDefense);
         }
         // else{
         //     meuItem.tipoArma = Item.tiposDeArma.espada;
         //     //
         //     if(!(meuItem.tipoItem==Item.tiposDeItem.ferramenta&&meuItem.tipoFerramenta==Item.tiposDeFerramenta.arco)){
         //         // meuItem.velocidade = 0f;
         //         // //
         //         // meuItem.physicDamage = 0f;
         //         // meuItem.thunderDamage = 0f;
         //         // meuItem.voidDamage = 0f;
         //         // meuItem.fireDamage = 0f;
         //         // meuItem.iceDamage = 0f;
         //     }
         //     //
         //     if(!(meuItem.tipoItem==Item.tiposDeItem.ferramenta&&meuItem.tipoFerramenta==Item.tiposDeFerramenta.escudo)){
         //         // meuItem.physicDefense = 0f;
         //         // meuItem.thunderDefense = 0f;
         //         // meuItem.voidDefense = 0f;
         //         // meuItem.fireDefense = 0f;
         //         // meuItem.iceDefense = 0f;
         //     }
         // }
         EditorGUILayout.EndFadeGroup();
         //Outro
         if(EditorGUILayout.BeginFadeGroup(outro.faded)){
             meuItem.itemID = EditorGUILayout.TextField("Item ID", meuItem.itemID);
         }
         
        EditorGUILayout.EndFadeGroup();
        
    }
 }*/
