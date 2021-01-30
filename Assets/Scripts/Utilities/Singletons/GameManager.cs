namespace redd096
{
    using UnityEngine;

    [AddComponentMenu("redd096/Singletons/Game Manager")]
    public class GameManager : Singleton<GameManager>
    {
        public UIManager uiManager { get; private set; }

        protected override void SetDefaults()
        {
            //get references
            uiManager = FindObjectOfType<UIManager>();


            //controlla se è la stessa scena di prima
            //avrà una lista di transform in cui piazzare
            //un prefab della tomba
            //si resetta quando cambia scena

            //ci sarà un sound manager che ogni volta che attiva un suono checka gli slider dei volumi
        }
    }
}