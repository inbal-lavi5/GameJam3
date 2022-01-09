using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour // Singleton<GameManager>
{
    [SerializeField] public int manaPickUpsToSpreadAtStart = 30;
    [SerializeField] public int pickUpsToSpread = 3;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;
    [SerializeField] public float randomLocationToInstantiate = 10;

    [SerializeField] public SoundManager SoundManager;
    
    private int level = 0;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city", "country"};

    private int destroyedItemsCounter = 0;
    // [SerializeField] private int numOfObjectsToDestroy = 3;

    [SerializeField] public Image objectToDestroy;
    [SerializeField] public List<Sprite> images;


    void Awake()
    {
        ResetItemsToDestroy();

        GameObject PickUpMana = (GameObject) Resources.Load("PickUpMana", typeof(GameObject));

        for (int i = 0; i < manaPickUpsToSpreadAtStart; i++)
        {
            GameObject pickupMana = Instantiate(PickUpMana);

            float posx = Random.Range(-160f, 160f);
            float posz = Random.Range(-260f, 400f);
            pickupMana.transform.position = new Vector3(posx, 4f, posz);
        }
    }

    private void ResetItemsToDestroy()
    {
        int randomGoal = Random.Range(0, images.Count);
        objectToDestroy.sprite = images[randomGoal];
    }

    public void InstantiatePickups(Transform location)
    {
        GameObject PickUp = (GameObject) Resources.Load("PickUp", typeof(GameObject));
        // int howMuchToCreate = Random.Range(1, 3);

        for (int i = 0; i < pickUpsToSpread; i++)
        {
            GameObject pickup = Instantiate(PickUp);

            float posx = Random.Range(location.position.x - randomLocationToInstantiate,
                location.position.x + randomLocationToInstantiate);
            float posz = Random.Range(location.position.z - randomLocationToInstantiate,
                location.position.z + randomLocationToInstantiate);
            pickup.transform.position = new Vector3(posx, 4f, posz);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }
    }

    public void Reset()
    {
        level = 0;
        SceneManager.LoadScene(levelsList[0]);
        ResetItemsToDestroy();
        destroyedItemsCounter = 0;
    }

    public void disableImage()
    {
        objectToDestroy.transform.parent.gameObject.SetActive(false); //todo might not be parent
    }

    public void NextLevel()
    {
        level++;
        SceneManager.LoadScene(levelsList[level]);
        //StartCoroutine(ExecuteAfterSceneLoaded());
    }

    IEnumerator ExecuteAfterSceneLoaded()
    {
        bool isLoaded = SceneManager.GetActiveScene().isLoaded;
        // SceneManager.sceneUnloaded
        yield return new WaitUntil(() => isLoaded);
        //InstantiatePickups("PickUpMana", manaPickUpsToSpreadAtStart);
    }


    public void NextItemToDestroy()
    {
        destroyedItemsCounter++;
        ResetItemsToDestroy();
    }

    /**
     * gets a destroyed item and checks if its the right item to destroy
     */
    public bool AddDestroyedItem(CustomTag tags, Transform location)
    {
        if (tags.HasTag(objectToDestroy.sprite.name))
        {
            InstantiatePickups(location);
            NextItemToDestroy();
            return true;
        }

        return false;
    }

    public void PlaySound(SoundManager.Sounds sfx)
    {
        SoundManager.PlaySound(sfx);
    }
}