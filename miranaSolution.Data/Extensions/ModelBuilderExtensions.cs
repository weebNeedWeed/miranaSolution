using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;

namespace miranaSolution.Data.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Slide>().HasData(
                new Slide
                {
                    Id = 1,
                    Name = "LINH VŨ THIÊN HẠ",
                    ShortDescription = "Lục Thiếu Du, linh hồn bị xuyên qua đến thế giới khác, nhập vào thân thể của một thiếu gia không có địa vị phải trải qua cuộc sống không khác gì nô bộc.\r\nThế giới này lấy Vũ vi cường, lấy Linh vi tôn, nghe đồn khi võ đạo đỉnh phong, linh đạo đạt đến cực hạn có thể phá toái hư không.",
                    ThumbnailImage = "https://static.8cache.com/cover/o/eJzLyTDW160wTC70dXI0zAnO1g9LL0gpsAz0CA_x1HeEAqckR31jj0A_n_Jg8ygXC_1yI0NT3QxjIyNdz2QTIwCuMBMz/linh-vu-thien-ha.jpg",
                    Genres = "Tiên Hiệp,Dị Giới,Huyền Huyễn,Xuyên Không",
                    SortOrder = 1,
                },
                new Slide
                {
                    Id = 2,
                    Name = "THẾ GIỚI HOÀN MỸ",
                    ShortDescription = "Một hạt bụi có thể lấp biển, một cọng cỏ chém hết mặt trời mặt trăng và ngôi sao, trong nháy mắt ở giữa long trời lỡ đất. Quần hùng cùng nổi lên, vạn tộc mọc lên san sát như rừng, chư thánh tranh bá, loạn khắp đất trời. Hỏi mặt đất bao la, cuộc đời thăng trầm? Một thiếu niên theo trong đất hoang đi ra, tất cả bắt đầu từ nơi này...",
                    ThumbnailImage = "https://static.8cache.com/cover/o/eJzLyTDT1y1Mcw2M0C0IMAvL1g9z8nUxMYwyD3Tz1HeEgmwfR_0SAzefTKOgCI8MC_1yQwsD3QwLAwAkvRE7/truyen-dau-pha-thuong-khung.jpg",
                    Genres = "Tiên Hiệp,Kiếm Hiệp,Huyền Huyễn",
                    SortOrder = 2,
                },
                new Slide
                {
                    Id = 3,
                    Name = "PHÀM NHÂN TU TIÊN",
                    ShortDescription = "Phàm Nhân Tu Tiên là một câu chuyện Tiên Hiệp kể về Hàn Lập - Một người bình thường nhưng lại gặp vô vàn cơ duyên để bước đi trên con đường tu tiên, không phải anh hùng - cũng chẳng phải tiểu nhân, Hàn Lập từng bước khẳng định mình... Liệu Hàn Lập và người yêu có thể cùng bước trên con đường tu tiên và có một cái kết hoàn mỹ? Những thử thách nào đang chờ đợi bọn họ?",
                    ThumbnailImage = "https://static.8cache.com/cover/o/eJzLyTDT17WITwqMNNQtNKp01A_zNXY1ifQuc8301HeEghwTR_1IV8PsTO-w4HKTUP1yI0NT3QxjIyMANRgRnA==/pham-nhan-tu-tien.jpg",
                    Genres = "Tiên Hiệp,Kiếm Hiệp",
                    SortOrder = 3,
                });
        }
    }
}