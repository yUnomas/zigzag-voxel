using System.Text;
using UnityEngine;
using UnityEngine.UI;
// key namespaces
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.EssentialKit;
// internal namespace
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;
using VoxelBusters.CoreLibrary;
using System;

namespace VoxelBusters.EssentialKit.Demo
{
    public class UtilitiesDemo : DemoActionPanelBase<UtilitiesDemoAction, UtilitiesDemoActionType>
    {
        #region Fields

        [SerializeField]
        private     InputField          m_idInputField      = null;

        #endregion

        #region Base class methods

        protected override void OnActionSelectInternal(UtilitiesDemoAction selectedAction)
        {
            switch (selectedAction.ActionType)
            {
                case UtilitiesDemoActionType.OpenAppStorePage:
                    Log("Opening app store page");
                    Utilities.OpenAppStorePage();
                    break;

                case UtilitiesDemoActionType.OpenCustomAppStorePage:
                    string  appId      = m_idInputField.text;
                    if (string.IsNullOrEmpty(appId))
                    {
                        Log("Provide application id.");
                        return;
                    }
                    Log("Opening app store page");
                    Utilities.OpenAppStorePage(appId);
                    break;

                case UtilitiesDemoActionType.OpenApplicationSettings:
                    Utilities.OpenApplicationSettings();
                    break;

                case UtilitiesDemoActionType.RequestInfoForAgeCompliance:
                    Log("Requesting info for age compliance.");
                    var options = new RequestInfoForAgeComplianceOptions.Builder()
                                    .AddContentAgeGateRange(0, 10)
                                    .AddContentAgeGateRange(11, 16)
                                    .AddContentAgeGateRange(17, 100)
                                    .Build();

                    Utilities.RequestInfoForAgeCompliance(options, OnInfoForAgeCompliance);

                    //With Mock Data
                    //Utilities.RequestInfoForAgeCompliance(options, OnInfoForAgeCompliance, new AgeComplianceMockData(new AgeRange(0, 10), AgeSharingStatus.Shared, AgeRangeDeclarationMethod.DeclaredBySelf));
                    break;

                case UtilitiesDemoActionType.ResourcePage:
                    ProductResources.OpenResourcePage(NativeFeatureType.kExtras);
                    break;

                default:
                    break;
            }
        }

        private void OnInfoForAgeCompliance(InfoForAgeCompliance result, Error error)
        {
            Log("Received callback for Info for age compliance.");
            if (error == null)
            {
                Log($"{result}");
                if (result.AgeSharingStatus == AgeSharingStatus.Unknown)
                {
                    Log("Age sharing status is not known.");
                }
                else
                {
                    Log("Age sharing status: " + result.AgeSharingStatus);
                }

                if (result.AgeSharingStatus == AgeSharingStatus.NotApplicable)
                {
                    Log("User is in a region where age compliance is not required. Just proceed as if there are no age compliance requirements.");
                }
                else
                {
                    Log("User is in a region where age compliance is may or may not be required.");
                    Log("User age range: " + result.UserAgeRange);
                    Log("User age sharing status: " + result.AgeSharingStatus);
                    Log("User age range declaration method: " + result.UserAgeRangeDeclarationMethod);

                    if (result.AgeSharingStatus == AgeSharingStatus.Shared)
                    {
                        Log("Consider User has declared the age range and block the age sensitive content based on the Age Range({result.UserAgeRange}) declared.");
                    }
                    else if(result.AgeSharingStatus == AgeSharingStatus.NotShared || result.AgeSharingStatus == AgeSharingStatus.Unknown || result.AgeSharingStatus == AgeSharingStatus.VerificationPending)
                    {
                        Log("Consider User has not declared the age range and block the age sensitive content considering the user is not adult.");
                    }
                }
            }
            else
            {
                Log("Error requesting Age Compliance info: " + error);
            }   
        }

        #endregion
    }
}
