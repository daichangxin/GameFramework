﻿//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using GameFramework.GameStructure.GameItems.ObjectModel;
using GameFramework.GameStructure.Game.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.GameStructure.GameItems.Components.AbstractClasses
{
    /// <summary>
    /// abstract base for showing a prefab from the selected item including updating to a new prefab when the selection changes.
    /// </summary>
    public abstract class GameItemContextBaseMultiRunnable : GameItemContextBase
    {
        /// <summary>
        /// Shared context that other GameItemContext's can reference
        /// </summary>
        public GameConfiguration.GameItemType GameItemType
        {
            get { return _gameItemType; }
            set { _gameItemType = value; }
        }
        [Tooltip("The context that we are working within for determining what GameItem to use or for reference from other items.")]
        [SerializeField]
        GameConfiguration.GameItemType _gameItemType;

        /// <summary>
        /// Setup
        /// </summary>
        protected virtual void Start()
        {
            if (Context.GetReferencedContextMode() == ObjectModel.GameItemContext.ContextModeType.Selected)
                GetIBaseGameItemManager().BaseSelectedChanged += SelectedChanged;
            RunMethod(true);
        }


        /// <summary>
        /// Destroy
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Context.GetReferencedContextMode() == ObjectModel.GameItemContext.ContextModeType.Selected)
                GetIBaseGameItemManager().BaseSelectedChanged -= SelectedChanged;
        }


        /// <summary>
        /// Returns the current GameItemManager as type IBaseGameItemManager
        /// </summary>
        /// <returns></returns>
        protected override IBaseGameItemManager GetIBaseGameItemManager()
        {
            switch (GameItemType)
            {
                case GameConfiguration.GameItemType.Character:
                    Assert.IsNotNull(GameManager.Instance.Characters, string.Format("Characters are not setup when referenced from {0}", this.GetType().Name));
                    return GameManager.Instance.Characters;
                case GameConfiguration.GameItemType.Level:
                    Assert.IsNotNull(GameManager.Instance.Levels, string.Format("Levels are not setup when referenced from {0}", this.GetType().Name));
                    return GameManager.Instance.Levels;
                case GameConfiguration.GameItemType.Player:
                    Assert.IsNotNull(GameManager.Instance.Players, string.Format("Players are not setup when referenced from {0}", this.GetType().Name));
                    return GameManager.Instance.Players;
                case GameConfiguration.GameItemType.World:
                    Assert.IsNotNull(GameManager.Instance.Worlds, string.Format("Worlds are not setup when referenced from {0}", this.GetType().Name));
                    return GameManager.Instance.Worlds;
                default:
                    return null;
            }
        }


        /// <summary>
        /// Called if we are reacting to selection changes and the selection changes.
        /// </summary>
        /// <param name="oldItem"></param>
        /// <param name="item"></param>
        protected virtual void SelectedChanged(GameItem oldItem, GameItem item)
        {
            RunMethod(false);
        }


        /// <summary>
        /// You should implement this method which is called from start and optionally if the selection chages.
        /// </summary>
        /// <param name="isStart"></param>
        public abstract void RunMethod(bool isStart = true);
    }
}