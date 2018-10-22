using System;
using System.Collections.Generic;
using Models.Actions;
using Models.Dialogue;
using Models.Dtos;
using TextEncoding;

namespace Models
{
    public partial class BootstrapEntity : FlexEntity
    {
        public BootstrapEntity(FlexEntityDto dto) : base(dto)
        {
        }

        public BootstrapEntity() : base()
        {
            Name = "Bootstrapper";
            IsHidden = true;
        }

        public override IRoomAction MainAction(IRoom room)
        {
            return new StartBaseViewAction(this);
        }

        public override void CalculateDialogue(IRoom room)
        {
        }

        public override TagString GetLookText()
        {
            return string.Empty.Tag();
        }
    }

    public class StartBaseViewAction : SimpleAction
    {
        public StartBaseViewAction(IRoomActor src)
        {
            Source = src;
        }

        public override IEnumerable<TagString> Execute(IRoom room)
        {
            return new TagString[]
            {
                "<> found.".Encode(Source, LinkColors.Player).Tag(new [] { UIResponseTag.ViewBase, UIResponseTag.HideHUD, UIResponseTag.HideNavBar }),
                //".".Tag(),
                //".".Tag(),
                //".".Tag(),
                //".".Tag(),
                //"System initializing...".Tag(new [] { UIResponseTag.ViewBase })
            };
        }
    }
}