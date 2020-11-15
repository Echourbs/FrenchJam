using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Item : MonoBehaviour
{

	private SistemaItens _sistemaItens;
    //
    public static bool ativo;
	//
	[Header("Basico")]
	public GameObject mao;
	public GameObject bag;
	public GameObject fundo;
    public GameObject pauseMenu;
	//
	[Header("Seleção e Troca de Itens")]
	public float distanciaItem = 70f;
	public Image itemTrocar;
	private int itemClick1;
	private int itemClick2;
	//
	[Header("Item Info")]
	public Transform itemInfoTransform;
	public Image imagemItem;
	public Text nomeItem;
	public Text descricaoItem;
	public Text raridade;
	public Text[] outrosInfo = new Text[4];
	//
	[Header("Slots")]
	public Image[] itensMao = new Image[4];
	public Image[] itensBag = new Image[32];
	public Text[] itensMaoText = new Text[4];
	public Text[] itensBagText = new Text[32];
	//
	private Item itemInfo;
	private bool escolheuItem;

    void Start()
    {
        _sistemaItens = MovementScript.player.GetComponent<SistemaItens>();
    	//
        distanciaItem = distanciaItem*(Screen.width/1280f);
        //
    	AtualizarInterface();
    }

    void Update(){
    	if(Input.GetButtonDown("Bag") && !pauseMenu.active)
        {
    		OnOffMenuItens();
    	}
    	if(Input.GetKeyDown("escape")){
    		StartCoroutine(SetOnOffMenuItem(false));
    	}
    	//
    	if(Input.GetButtonDown("Click 0")&&ativo){
    		itemClick1 = ProcurarItemMaisProximo(true);
    	}
    	else if(Input.GetButtonUp("Click 0")&&escolheuItem&&ativo){
    		itemClick2 = ProcurarItemMaisProximo(true);
    		//
    		if(escolheuItem){
    			if(itemClick1<itensBag.Length&&itemClick2<itensBag.Length){
    				_sistemaItens.TrocarPosicaoItemBag(itemClick1, itemClick2);
    			}
    			else if(itemClick1>=itensBag.Length&&itemClick2<itensBag.Length){
    				_sistemaItens.TrocarItemMao(itemClick2, itemClick1-itensBag.Length);
    			}
    			else if(itemClick1<itensBag.Length&&itemClick2>=itensBag.Length){
    				_sistemaItens.TrocarItemMao(itemClick1, itemClick2-itensBag.Length);
    			}
    			else{
    				_sistemaItens.TrocarItensMao(itemClick1-itensBag.Length, itemClick2-itensBag.Length);
    			}
    			AtualizarInterface();
    		}
    		//
    		escolheuItem = false;
    	}
    	//
    	if(Input.GetButtonDown("Click 1")&&!escolheuItem&&ativo){
    		int itemProximo = ProcurarItemMaisProximo(false);
    		if(itemProximo<itensBag.Length){
    			_sistemaItens.DroparItem(itemProximo, false);
    		}
    		else{
    			_sistemaItens.DroparItem(itemProximo-itensBag.Length, true);
    		}
    		AtualizarInterface();
    	}
    	//
    	if(ativo&&escolheuItem&&itemInfo!=null){
    		itemTrocar.sprite = itemInfo.imagem;
    		itemTrocar.color = new Color(1f, 1f, 1f, 0.5f);
    		//
    		itemTrocar.transform.position = CursorInput.posMouse;
    	}
    	else{
    		itemTrocar.sprite = null;
    		itemTrocar.color = Color.clear;
    	}
    	//
    	if(ativo&&!escolheuItem){
    		ProcurarItemMaisProximo(false);
    		//
    		if(itemInfo!=null){
    			itemInfoTransform.gameObject.SetActive(true);
    			DescreverItemInfo(itemInfo);
    		}
    		else{
    			itemInfoTransform.gameObject.SetActive(false);
    		}
    		//
    		itemInfoTransform.position = CursorInput.posMouse;
    	}
    }

    void OnOffMenuItens(){
    	ativo = !ativo;
    	bag.SetActive(ativo);
    	mao.SetActive(ativo);
    	fundo.SetActive(ativo);
        CursorInput.ativo = ativo;
    	//
    	itemInfoTransform.gameObject.SetActive(ativo);
    	itemTrocar.gameObject.SetActive(ativo);
    	//
    	AtualizarInterface();
    }

    IEnumerator SetOnOffMenuItem(bool _ativo){
    	yield return new WaitForEndOfFrame();
    	ativo = _ativo;
    	bag.SetActive(ativo);
    	mao.SetActive(ativo);
    	fundo.SetActive(ativo);
        CursorInput.ativo = ativo;
    	//
    	itemInfoTransform.gameObject.SetActive(ativo);
    	itemTrocar.gameObject.SetActive(ativo);
    	//
    	AtualizarInterface();
    }

    //__________________________________________
    //Acoes Interface

    int ProcurarItemMaisProximo(bool informarSeFuncionou){
    	Vector2 posMouse = CursorInput.posMouse;
    	bool encontrouItemProximo = false;
    	int indexItemMaisProximo = 0;
    	//
    	float distancia = 0;
    	float menorDistancia = distanciaItem;
    	//
    	for(int i=0; i<itensMao.Length; i++){
    		Vector2 posItem = itensMao[i].GetComponent<RectTransform>().position;
    		//
    		distancia = Vector3.Distance(posMouse, posItem);
    		//
    		if(distancia<=distanciaItem){
    			if(distancia<menorDistancia){
    				menorDistancia = distancia;
    				indexItemMaisProximo = i+itensBag.Length;
    				itemInfo = _sistemaItens.itensMao[i].item;
    				encontrouItemProximo = true;
    			}
    		}
    	}
    	//
	    for(int i=0; i<itensBag.Length; i++){
	    	Vector2 posItem = itensBag[i].GetComponent<RectTransform>().position;
	    	//
	    	distancia = Vector2.Distance(posMouse, posItem);
	    	//
	    	if(distancia<=distanciaItem){
	    		if(distancia<menorDistancia){
	    			menorDistancia = distancia;
	    			indexItemMaisProximo = i;
	    			itemInfo = _sistemaItens.itensBag[i].item;
	    			encontrouItemProximo = true;
	    		}
	    	}
	    }
    	//
    	// print(menorDistancia);
    	//
    	if(informarSeFuncionou){
    		escolheuItem = encontrouItemProximo;
    	}
    	return indexItemMaisProximo;
    }

    //__________________________________________
    //ItemInfo

    void DescreverItemInfo(Item item){
    	imagemItem.sprite = item.imagem;
    	nomeItem.text = item.nome;
    	descricaoItem.text = item.descricao;
    	//
    	string valorRaridade = "";
    	switch(item.raridade){
    		case Item.tiposRaridade.comum:
    			valorRaridade = "comum";
    			break;
    		case Item.tiposRaridade.incomum:
    			valorRaridade = "incomum";
    			break;
    		case Item.tiposRaridade.raro:
    			valorRaridade = "raro";
    			break;
    		case Item.tiposRaridade.epico:
    			valorRaridade = "épico";
    			break;
    		case Item.tiposRaridade.lendario:
    			valorRaridade = "lendário";
    			break;
    		default:
    			break;
    	}
    	raridade.text = valorRaridade;
    	//
    	int valorItem = 0;
    	switch(item.tipoItem){
    		case Item.tiposDeItem.consumivel:
    			valorItem = 0;
    			break;
    		case Item.tiposDeItem.chave:
    			valorItem = 1;
    			break;
    		case Item.tiposDeItem.arma:
    			valorItem = 2;
    			break;
    		default:
    			break;
    	}
    	//
    	if(valorItem==0){
    		for(int i=0; i<outrosInfo.Length; i++){
    			outrosInfo[i].text = "";
    		}
    	}
    	else if(valorItem==1){
    		for(int i=0; i<outrosInfo.Length; i++){
    			outrosInfo[i].text = "";
    		}
    	}
    	else if(valorItem==2){
    		string valorArma = "";
            valorItem = 0;
    		switch(item.tipoArma){
    			case Item.tiposDeArma.espada:
    				valorArma = "espada";
    				break;
    			case Item.tiposDeArma.adaga:
    				valorArma = "adaga";
    				break;
    			case Item.tiposDeArma.martelo:
    				valorArma = "martelo";
    				break;
    			case Item.tiposDeArma.bastao:
    				valorArma = "bastao";
    				break;
    			case Item.tiposDeArma.machado:
    				valorArma = "machado";
    				break;
                case Item.tiposDeArma.escudo:
                    valorArma = "escudo";
                    valorItem = 1;
                    break;
                case Item.tiposDeArma.arco:
                    valorArma = "arco";
                    valorItem = 2;
                    break;
                case Item.tiposDeArma.gancho:
                    valorArma = "gancho";
                    valorItem = 3;
                    break;
    			default:
    				break;
    		}
    		//
    		outrosInfo[0].text = "tipo de arma: "+valorArma;
    		//
            if(valorItem==0){
                outrosInfo[1].text = "velocidade: "+item.velocidade;
                //
                outrosInfo[2].text = "Danos: PD: "+item.physicDamage+"/TD: "+item.thunderDamage+"/VD: "+item.voidDamage+"/FD: "+item.fireDamage+"/ID: "+item.iceDamage;
                outrosInfo[3].text = "Defesas: PD: "+item.physicDefense+"/TD: "+item.thunderDefense+"/VD: "+item.voidDefense+"/FD: "+item.fireDefense+"/ID: "+item.iceDefense;
            }
            else if(valorItem==1){
                outrosInfo[1].text = "Defesas: PD: "+item.physicDefense+"/TD: "+item.thunderDefense+"/VD: "+item.voidDefense+"/FD: "+item.fireDefense+"/ID: "+item.iceDefense;
                //
                outrosInfo[2].text = "";
                outrosInfo[3].text = "";
            }
            else if(valorItem==2){
                outrosInfo[1].text = "velocidade: "+item.velocidade;
                //
                outrosInfo[2].text = "Danos: PD: "+item.physicDamage+"/TD: "+item.thunderDamage+"/VD: "+item.voidDamage+"/FD: "+item.fireDamage+"/ID: "+item.iceDamage;
                //
                outrosInfo[3].text = "";
            }
            else{
                for(int i=0; i<outrosInfo.Length; i++){
                    outrosInfo[i].text = "";
                }
            }
    	}
    	else{
    		print("isso não pode ser um Item");
    	}
    }

    void AtualizarInterface()
    {
    	if(itemInfo!=null){
    		DescreverItemInfo(itemInfo);
    	}
    	//
    	for(int i=0; i<itensMao.Length; i++){
    		if(_sistemaItens.itensMao[i].item!=null){
    			itensMao[i].sprite = _sistemaItens.itensMao[i].item.imagem;
    			itensMao[i].color = Color.white;
    			//
    			itensMaoText[i].text = (_sistemaItens.itensMao[i].quantos+1).ToString();
    		}
    		else{
    			itensMao[i].sprite = null;
    			itensMao[i].color = Color.clear;
    			//
    			itensMaoText[i].text = "";	
    		}
        }
    	//
        for(int i=0; i<itensBag.Length; i++){
        	if(_sistemaItens.itensBag[i].item!=null){
        		itensBag[i].sprite = _sistemaItens.itensBag[i].item.imagem;
        		itensBag[i].color = Color.white;
        		//
        		itensBagText[i].text = (_sistemaItens.itensBag[i].quantos+1).ToString();
        	}
        	else{
        		itensBag[i].sprite = null;
        		itensBag[i].color = Color.clear;
        		//
        		itensBagText[i].text = "";	
        	}
        }
    }
}
