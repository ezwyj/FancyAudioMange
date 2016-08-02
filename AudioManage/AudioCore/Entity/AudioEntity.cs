using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PetaPoco;
using DogNet.Repositories;
using Common.Entity;

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

        public int orderNum { get; set; }


        public string QrCodeFile { get; set; }

        public string Remark { get; set; }

        public int State { get; set; }

        public string Content { get; set; }
        [PetaPoco.Ignore]
        public string ContentMini { 
            get 
            { 
                if (Content!=null)
                {
                    return Content.Length > 40 ? Content.Substring(0, 40) + "..." : Content; 
                }
                else
                {
                    return "";
                }
            
            } 
        }

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

        [PetaPoco.Ignore]
        public List<AttachmentEntity> AudioFile
        {
            get
            {
                List<AttachmentEntity> ret = new List<AttachmentEntity>();
                if (AudioFileId!=null)
                {
                    Sql sql = new Sql();
                    sql.Append("Select * from attachment where id in (");
                    sql.Append(AudioFileId==""?"0":AudioFileId);
                    sql.Append(")");
                    return AttachmentEntity.DefaultDB.Fetch<AttachmentEntity>(sql);
                   
                }

                return null;
            }
        }
    }
}
