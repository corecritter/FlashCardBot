using Core.Model;
using System;
using System.Linq;
using Xunit;

namespace Core.Test
{
    public class ManageDeckSlotTypeTests
    {
        [Fact]
        public void Validate()
        {
            ManageDeckSlotType model = new ManageDeckSlotType() { };

            var errors = model.Validate();
            Assert.Equal(1, errors.Count());

            model.ManageType = Constants.ManageTypes.Add.ToString();
            errors = model.Validate();
            Assert.Equal(0, errors.Count());

            model.DeckName = "DeckName";
            Assert.Equal(0, model.Validate().Count());

            model.ManageType = Constants.ManageTypes.Modify.ToString();
            model.DeckName = "DeckName";
            Assert.Equal(2, model.Validate().Count());
        }

        [Fact]
        public void SlotToElicit()
        {
            ManageDeckSlotType model = new ManageDeckSlotType() { };

            model.ManageType = Constants.ManageTypes.Modify.ToString();

            string slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.DeckName), slotToElicit);

            model.DeckName = "DeckName";
            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.Front), slotToElicit);

            model.Front = "Front";
            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.Back), slotToElicit);

            model.Back = "Back";
            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.Confirm), slotToElicit);

            model.DeckName = null;
            model.Front = null;
            model.Back = null;

            model.ManageType = Constants.ManageTypes.Delete.ToString();

            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.DeckName), slotToElicit);

            model.DeckName = "DeckName";
            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.Confirm), slotToElicit);

            model.Confirm = "Yes";
            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(String.Empty, slotToElicit);

            model.Confirm = null;
            model.ManageType = Constants.ManageTypes.Add.ToString();
            slotToElicit = model.GetSlotToElicit();
            Assert.Equal(nameof(ManageDeckSlotType.Confirm), slotToElicit);
        }

    }
}
