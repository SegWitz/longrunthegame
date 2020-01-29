using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace Scripts.InAppPurchase
{
    public class InAppPurchaseController : MonoBehaviour, IStoreListener
    {
        #region public variables

//		public Text currentRedStars;

        /// <summary>
        /// Product id.
        /// </summary>
        [Space(10f)]
        public static string ProductIdConsumable = "redstar001";

        /// <summary>
        /// Button to purchase redstar.
        /// </summary>
        [Header("Connect the button for purchasing")]
        [Space(20f)]
        public Button RedStarPurchaseButton; 


        #endregion

        #region Private variables

        /// <summary>
        /// Unity purchasing system static reference.
        /// </summary>
        private static IStoreController _storeController;

        /// <summary>
        /// Store Extension Provider static reference.
        /// </summary>
        private static IExtensionProvider _storeExtensionProvider;

        #endregion

        #region Interface Implementation

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("OnInitialized: PASS");

            // Overall Purchasing system, configured with products for this application.
            _storeController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            _storeExtensionProvider = extensions;
        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        }


        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // A consumable product has been purchased by this user.
            if (string.Equals(args.purchasedProduct.definition.id, ProductIdConsumable, StringComparison.Ordinal))
            {
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
                // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
                //TODO:
				TheRunGameManager.Instance.GameData.Data.Profile.RedStars += 100;
				UMP_Manager.Reference.UpdateRedStarsText();
				TheRunGameManager.Instance.GameData.Save();
				//PurchaseProcessingResult.Pending;
                //Add 100 Red Star Here.
            }
            // Or ... an unknown product has been purchased by this user. Fill in additional products here....
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
            }

            // Return a flag indicating whether this product has completely been received, or if the application needs 
            // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
            // saving purchased products to the cloud, and when that save is delayed. 
            return PurchaseProcessingResult.Complete;
        }


        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
            // this reason with the user to guide their troubleshooting actions.
            Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        }

        #endregion

        #region Unity Functions
        /// <summary>
        /// Awake the instance.
        /// </summary>
        private void Awake()
        {
            //Remove if any old listener allready attached.
            RedStarPurchaseButton.onClick.RemoveAllListeners();
            //Hook with purchasing consumable product.
            RedStarPurchaseButton.onClick.AddListener(BuyConsumable);
        }

        /// <summary>
        /// Start the instance.
        /// </summary>
        private void Start()
        {
            // If we haven't set up the Unity Purchasing reference
            if (_storeController == null)
            {
                // Begin to configure our connection to Purchasing
                InitializePurchasing();
            }
        }

        #endregion

        #region Normal Function

        /// <summary>
        /// Initialize purchase.
        /// </summary>
        public void InitializePurchasing()
        {
            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add a product to sell / restore by way of its identifier, associating the general identifier
            // with its store-specific identifiers.
            builder.AddProduct(ProductIdConsumable, ProductType.Consumable);
            /*
            // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
            // if the Product ID was configured differently between Apple and Google stores. Also note that
            // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
            // must only be referenced here. 
            builder.AddProduct(ProductIdConsumable, ProductType.Consumable,
                new IDs() {{ProductIdConsumable, GooglePlay.Name}});
                */

           

            // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
            // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Is initialized.
        /// </summary>
        /// <returns></returns>
        private static bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return _storeController != null && _storeExtensionProvider != null;
        }

        /// <summary>
        /// Purchase consumable product.
        /// </summary>
        public void BuyConsumable()
        {
            // Buy the consumable product using its general identifier. Expect a response either 
            // through ProcessPurchase or OnPurchaseFailed asynchronously.
            BuyProductId(ProductIdConsumable);
        }

        /// <summary>
        /// Purchase product with id.
        /// </summary>
        /// <param name="productId"></param>
        private void BuyProductId(string productId)
        {
            // If Purchasing has been initialized ...
            if (IsInitialized())
            {
                // ... look up the Product reference with the general product identifier and the Purchasing 
                // system's products collection.
                var product = _storeController.products.WithID(productId);

                // If the look up found a product for this device's store and that product is ready to be sold ... 
                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                    // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
                    // asynchronously.
                    _storeController.InitiatePurchase(product);
                }
                // Otherwise ...
                else
                {
                    // ... report the product look-up failure situation  
                    Debug.Log(
                        "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            // Otherwise ...
            else
            {
                // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
                // retrying initiailization.
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }

        #endregion
    }
}