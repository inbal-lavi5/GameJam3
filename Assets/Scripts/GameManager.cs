using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] public int manaPickUpsToSpreadAtStart = 30;
    [SerializeField] public int pickUpsToSpread = 15;
    [SerializeField] public int pickUpsToCollectTillExplosion = 5;
    [SerializeField] public float randomLocationToInstantiate = 10;

    [SerializeField] public SoundManager SoundManager;

    [SerializeField] private int level = 1;

    [SerializeField] public List<string> levelsList = new List<string>
        {"city tutorial", "city", "country"};

    [SerializeField] public Image objectToDestroy;
    [SerializeField] public Animator objectToDestroyAnimator;
    [SerializeField] public List<Sprite> images;


    void Awake()
    {
        NextItemsToDestroy();
        spreadItems("PickUp", -160, 160, -260, 400, pickUpsToSpread);
        spreadItems("PickUp1", -160, 160, -260, 400, 20);
        spreadItems("PickUpMana", -160, 160, -260, 400, manaPickUpsToSpreadAtStart);

    }
    
    protected virtual void NextItemsToDestroy()
    {
        // int randomGoal = Random.Range(0, images.Count);
        // objectToDestroy.sprite = images[randomGoal];
        // print(objectToDestroy.sprite.name);

        // objectToDestroyAnimator.Play("Tree");
        // objectToDestroyAnimator.Play(objectToDestroy.sprite.name, -1, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //todo remove at end
        {
            Reset();
        }
    }

    public void Reset()
    {
        level = 1;
        SceneManager.LoadScene(levelsList[level]);
        NextItemsToDestroy();
    }

    public void spreadItems(String item, float xMin, float xMax, float zMin, float zMax, int amount)
    {
        GameObject PickUp = (GameObject) Resources.Load(item, typeof(GameObject));

        for (int i = 0; i < amount; i++)
        {
            GameObject pickup = Instantiate(PickUp);
            pickup.transform.position = generateLocation(xMin, xMax, zMin, zMax);
        }
    }

    private Vector3 generateLocation(float xMin, float xMax, float zMin, float zMax)
    {
        float posx = Random.Range(xMin, xMax);
        float posz = Random.Range(zMin, zMax);
        return new Vector3(posx, 4f, posz);
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
        // InstantiatePickups("PickUpMana", manaPickUpsToSpreadAtStart);
    }


    /**
     * gets a destroyed item and checks if its the right item to destroy
     */
    public bool AddDestroyedItem(CustomTag tags, Vector3 location)
    {
        if (tags.HasTag(objectToDestroy.sprite.name))
        {
            // spreadItems("PickUp", location.x - randomLocationToInstantiate, location.x + randomLocationToInstantiate, location.z - randomLocationToInstantiate, location.z + randomLocationToInstantiate, pickUpsToSpread);
            // NextItemsToDestroy();
            return true;
        }

        return false;
    }

    public void PlaySound(SoundManager.Sounds sfx)
    {
        SoundManager.PlaySound(sfx);
    }
}