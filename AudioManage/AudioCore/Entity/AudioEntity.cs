using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using DogNet.Repositories;

namespace AudioCore.Entity
{
     [RepositoryEntity(DefaultConnName = "POV")]
    [PetaPoco.TableName("audio")]
    [PetaPoco.PrimaryKey("Id")]
    public class AudioEntity: Repository<AudioEntity>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }

        public int Order { get; set; }

        public string AudioFile { get; set; }
        public string QrCodeFile { get; set; }

        public string Remark { get; set; }

        public int State { get; set; }

        public string Content { get; set; }
        [PetaPoco.Ignore]
        public string ContentMini { get { return Content.Length > 20 ? Content.Substring(0, 20) + "..." : Content; } }

        public DateTime CreateTime { get; set; }

        [PetaPoco.Ignore]
        public string CreateTimeExp
        {
            get { return CreateTime.ToString(); }

        }

        [PetaPoco.Ignore]
        public string StateExp
        {
            get
            {
                string ret = "";
                switch (State)
                {
                    case 1:
                        ret = "正在使用";
                        break;
                    case 2:
                        ret = "未知";
                        break;
                        
                }
                return ret;
            }
        }

        public string AudioFileId { get; set; }
    }
}
