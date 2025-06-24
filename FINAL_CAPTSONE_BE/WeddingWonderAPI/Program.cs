using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using DataAccess.DataAccess;
using Fleck;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Repository.IRepositories;
using Repository.Repositories;
using Services;
using Services.Email;
using Services.Services;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using WeddingWonderAPI.BGService;
using WeddingWonderAPI.Dependency.Extension;
namespace WeddingWonderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<WeddingWonderDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("WeddingWonderDb"),
                    new MySqlServerVersion(new Version(9, 0, 1))));

            // Đăng ký các DAO
            builder.Services.AddScoped<UserDAO>();
            builder.Services.AddScoped<TokenDAO>();
            builder.Services.AddScoped<ClothesServiceDAO>();
            builder.Services.AddScoped<OutfitDAO>();
            builder.Services.AddScoped<CustomerReviewDAO>();
            builder.Services.AddScoped<ServiceDAO>();
            builder.Services.AddScoped<SingleBookingDAO>();
            builder.Services.AddScoped<InForBookingDAO>();
            builder.Services.AddScoped<InvitationPackageDAO>();
            builder.Services.AddScoped<PhotographServiceDAO>();
            builder.Services.AddScoped<PhotographPackageDAO>();
            builder.Services.AddScoped<MakeUpServiceDAO>();
            builder.Services.AddScoped<MakeUpPackageDAO>();
            builder.Services.AddScoped<RestaurantServiceDAO>();
            builder.Services.AddScoped<EventOrganizerServiceDAO>();
            builder.Services.AddScoped<InvitationServiceDAO>();
            builder.Services.AddScoped<EventPackageDAO>();
            builder.Services.AddScoped<CateringDAO>();
            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<IUrlHelperFactory, UrlHelperFactory>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<InForAdminDAO>();
            builder.Services.AddScoped<MessageDAO>();
            builder.Services.AddScoped<VoucherAdminDAO>();
            builder.Services.AddScoped<BusyScheduleDAO>();
            builder.Services.AddScoped<OutfitImageDAO>();
            builder.Services.AddScoped<ServiceImageDAO>();
            builder.Services.AddScoped<AdminLogDAO>();
            builder.Services.AddScoped<ComboBookingDAO>();
            builder.Services.AddScoped<BookingServiceDetailDAO>();
            builder.Services.AddScoped<MenuDAO>();
            builder.Services.AddScoped<EventConceptDAO>();
            builder.Services.AddScoped<TransactionDAO>();
            builder.Services.AddScoped<FavoriteServiceDAO>();
            builder.Services.AddScoped<ContractDAO>();
            builder.Services.AddScoped<OutfitOutfitTypeDAO>();
            builder.Services.AddScoped<BlogDAO>();
            builder.Services.AddScoped<OutfitTypeDAO>();
            builder.Services.AddScoped<NotificationDAO>();
            builder.Services.AddScoped<RequestUpgradeSupplierDAO>();
            builder.Services.AddScoped<RequestImageDAO>();
            builder.Services.AddScoped<MakeUpArtistDAO>();
            builder.Services.AddScoped<PhotographerDAO>();

            // Đăng ký các Repository
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IClothesServiceRepository, ClothesRepository>();
            builder.Services.AddScoped<IOutfitRepository, OutfitRepository>();
            builder.Services.AddScoped<ICustomerReviewRepository, CustomerReviewRepository>();
            builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
            builder.Services.AddScoped<ISingleBookingRepository, SingleBookingRepository>();
            builder.Services.AddScoped<IInForBookingRepository, InForBookingRepository>();
            builder.Services.AddScoped<IEventOrganizerServiceRepository, EventOrganizerServiceRepository>();
            builder.Services.AddScoped<IInvitationServiceRepository, InvitationServiceRepository>();
            builder.Services.AddScoped<IPhotographServiceRepository, PhotographServiceRepository>();
            builder.Services.AddScoped<IMakeUpServiceRepository, MakeUpServiceRepository>();
            builder.Services.AddScoped<IPhotographPackageRepository, PhotographPackageRepository>();
            builder.Services.AddScoped<IEventPackageRepository, EventPackageRepository>();
            builder.Services.AddScoped<IMakeUpPackageRepository, MakeUpPackageRepository>();
            builder.Services.AddScoped<IInvitationPackageRepository, InvitationPackageRepository>();
            builder.Services.AddScoped<IRestaurantServiceRepository, RestaurantServiceRepository>();
            builder.Services.AddScoped<ICateringRepository, CateringRepository>();
            builder.Services.AddScoped<IInForBookingRepository, InForBookingRepository>();
            builder.Services.AddScoped<IInForAdminRepository, InForAdminRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IVoucherAdminRepository, VoucherAdminRepository>();
            builder.Services.AddScoped<IInForAdminRepository, InForAdminRepository>();
            builder.Services.AddScoped<IMessageRepository, MessageRepository>();
            builder.Services.AddScoped<IVoucherAdminRepository, VoucherAdminRepository>();
            builder.Services.AddScoped<IBusyScheduleRepository, BusyScheduleRepository>();
            builder.Services.AddScoped<IOutfitImageRepository, OutfitImageRepository>();
            builder.Services.AddScoped<IServiceImageRepository, ServiceImageRepository>();
            builder.Services.AddScoped<IAdminLogRepository, AdminLogRepository>();
            builder.Services.AddScoped<IComboBookingRepository, ComboBookingRepository>();
            builder.Services.AddScoped<IBookingServiceDetailRepository, BookingServiceDetailRepository>();
            builder.Services.AddScoped<IMenuRepository, MenuRepository>();
            builder.Services.AddScoped<IEventConceptRepository, EventConceptRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IFavoriteServiceRepository, FavoriteServiceRepository>();
            builder.Services.AddScoped<IContractRepository, ContractRepository>();
            builder.Services.AddScoped<IOutfitOutfitTypeRepository, OutfitOutfitTypeRepository>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IOutfitTypeRepository, OutfitTypeRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IRequestUpgradeSupplierRepository, RequestUpgradeSupplierRepository>();
            builder.Services.AddScoped<IRequestImageRepository, RequestImageRepository>();
            builder.Services.AddScoped<IMakeUpArtistRepository, MakeUpArtistRepository>();
            builder.Services.AddScoped<IPhotographerRepository, PhotographerRepository>();

            // Đăng ký các Service
            builder.Services.AddScoped<BusyScheduleService>();
            builder.Services.AddScoped<Services.BGService>();
            builder.Services.AddScoped<ClothesServiceManager>();
            builder.Services.AddScoped<EventOrganizeServiceManager>();
            builder.Services.AddScoped<InForAdminService>();
            builder.Services.AddScoped<InvitationServiceManager>();
            builder.Services.AddScoped<MakeUpServiceManager>();
            builder.Services.AddScoped<MessageService>();
            builder.Services.AddScoped<PhotographServiceManager>();
            builder.Services.AddScoped<RestaurantServiceManager>();
            builder.Services.AddScoped<ServiceManager>();
            builder.Services.AddScoped<SingleBookingService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<VoucherAdminService>();
            builder.Services.AddScoped<ReviewService>();
            builder.Services.AddScoped<ClothesService>();
            builder.Services.AddScoped<AdminLogService>();
            builder.Services.AddScoped<ComboBookingService>();
            builder.Services.AddScoped<BookingService>();
            builder.Services.AddScoped<MenuService>();
            builder.Services.AddScoped<EventConceptServiceManager>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<TopicDAO>();
            builder.Services.AddScoped<TopicManager>();
            builder.Services.AddScoped<VnPayService>();
            builder.Services.AddScoped<TransactionService>();
            builder.Services.AddScoped<FavoriteServiceService>();
            builder.Services.AddScoped<BlogService>();
            builder.Services.AddScoped<MakeUpArtistService>();
            builder.Services.AddScoped<PhotographerService>();
            builder.Services.AddScoped<Services.ContractService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<RequestUpgradeSupplierService>();
            _ = builder.Services.AddScoped<ICloudStorageRepository>(provider =>
            new CloudStorageService(builder.Configuration.GetConnectionString("AzureBlobStorage")));

            builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("ElasticSearchSettings"));
            builder.Services.AddSingleton<IHostedService, EmailBGService>();
            builder.Services.AddSingleton<IElasticService, ElasticService>();
            // Cấu hình Authentication và JWT
            builder.Services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddHealthChecks();
            builder.Services.AddConfigureMasstransitRabbitMQ(builder.Configuration);
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true; // Optional: for pretty printing
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpClient();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WeddingWonderAPI", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Copy chỗ login rồi nhập access token zo đây",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            ConcurrentDictionary<int, IWebSocketConnection> wsConnections = new();
            builder.Services.AddSingleton(wsConnections);
            WebApplication app = builder.Build();
            IServiceScopeFactory scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>(); // Use scope factory for WebSocket

            // WebSocket server configuration
            WebSocketServer server = new("ws://0.0.0.0:8181");

            // JWT token handler configuration
            JwtSecurityTokenHandler tokenHandler = new();
            string? jwtSecret = builder.Configuration["JWT:Secret"];
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ClockSkew = TimeSpan.Zero
            };

            server.Start(ws =>
            {
                ws.OnOpen = async () =>
                {
                    Uri uri = new("ws://localhost:8181" + ws.ConnectionInfo.Path);
                    Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
                    string? token = query.ContainsKey("token") ? query["token"].ToString() : null;

                    if (string.IsNullOrEmpty(token))
                    {
                        ws.Close();
                        return;
                    }

                    try
                    {
                        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken? validatedToken);
                        Claim? userIdClaim = principal.FindFirst("Id") ?? principal.FindFirst(ClaimTypes.NameIdentifier);

                        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                        {
                            ws.Close();
                            return;
                        }

                        if (!wsConnections.TryAdd(userId, ws))
                        {
                            wsConnections[userId] = ws;
                        }

                        using (IServiceScope scope = scopeFactory.CreateScope())
                        {
                            UserService userService = scope.ServiceProvider.GetRequiredService<UserService>();
                            await userService.SetUserOnlineStatus(userId, true);

                            var statusMessage = new { type = "status", UserId = userId, IsOnline = true };
                            foreach (IWebSocketConnection connection in wsConnections.Values)
                            {
                                _ = connection.Send(JsonConvert.SerializeObject(statusMessage));
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ws.Close();
                    }
                };

                ws.OnClose = async () =>
                {
                    int userId = wsConnections.FirstOrDefault(c => c.Value == ws).Key;
                    if (userId != 0)
                    {
                        wsConnections.TryRemove(userId, out _);

                        using (IServiceScope scope = scopeFactory.CreateScope())
                        {
                            UserService userService = scope.ServiceProvider.GetRequiredService<UserService>();
                            await userService.SetUserOnlineStatus(userId, false);

                            var statusMessage = new { type = "status", UserId = userId, IsOnline = false };
                            foreach (IWebSocketConnection connection in wsConnections.Values)
                            {
                                _ = connection.Send(JsonConvert.SerializeObject(statusMessage));
                            }
                        }
                    }
                };

                ws.OnMessage = async message =>
                {
                    using (IServiceScope scope = scopeFactory.CreateScope())
                    {
                        MessageDAO messageDAO = scope.ServiceProvider.GetRequiredService<MessageDAO>();
                        IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                        try
                        {
                            BusinessObject.Models.Message? chatMessage = JsonConvert.DeserializeObject<BusinessObject.Models.Message>(message);

                            if (chatMessage == null)
                            {
                                _ = ws.Send("Invalid message format.");
                                return;
                            }

                            int senderId = wsConnections.FirstOrDefault(c => c.Value == ws).Key;

                            if (senderId == 0)
                            {
                                _ = ws.Send("Sender not recognized.");
                                return;
                            }
                            if (!string.IsNullOrEmpty(chatMessage.AttachmentUrl))
                            {
                                string extension = Path.GetExtension(chatMessage.AttachmentUrl).ToLower();
                                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                                {
                                    chatMessage.Type = "image";
                                }
                                else if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                                {
                                    chatMessage.Type = "document";
                                }
                                else
                                {
                                    chatMessage.Type = "text";
                                }
                            }
                            if (chatMessage.Type == "location" && !string.IsNullOrEmpty(chatMessage.Content))
                            {
                                string[] coordinates = chatMessage.Content.Split(',');
                                if (coordinates.Length != 2 || !double.TryParse(coordinates[0], out _) || !double.TryParse(coordinates[1], out _))
                                {
                                    _ = ws.Send("Invalid location format.");
                                    return;
                                }
                            }
                            else
                            {
                                chatMessage.Type = "text";
                            }
                            BusinessObject.Models.Message newMessage = new()
                            {
                                SenderId = senderId,
                                ReceiverId = chatMessage.ReceiverId,
                                Content = chatMessage.Content,
                                SendDate = DateTime.UtcNow,
                                AttachmentUrl = chatMessage.AttachmentUrl,
                                Type = chatMessage.Type,
                                ReplyToMessageId = chatMessage.ReplyToMessageId
                            };

                            await messageDAO.CreateMessage(newMessage);

                            MessageNotificationEvent notificationEvent = new()
                            {
                                Id = Guid.NewGuid(),
                                TimeStamp = DateTimeOffset.UtcNow,
                                SenderId = senderId,
                                ReceiverId = chatMessage.ReceiverId,
                                MessageContent = chatMessage.Content,
                                NotificationType = "Message",
                                Name = "New Message Notification",
                                Description = "You have a new message."
                            };

                            await publishEndpoint.Publish(notificationEvent);

                            var response = new
                            {
                                type = "message",
                                SenderId = senderId,
                                ReceiverId = chatMessage.ReceiverId,
                                Content = chatMessage.Content,
                                AttachmentUrl = chatMessage.AttachmentUrl,
                                Type = chatMessage.Type,
                                SendDate = DateTime.UtcNow,
                                ReplyToMessageId = chatMessage.ReplyToMessageId
                            };

                            if (wsConnections.TryGetValue(chatMessage.ReceiverId, out IWebSocketConnection? receiverConnection))
                            {
                                _ = receiverConnection.Send(JsonConvert.SerializeObject(response));
                            }

                            _ = ws.Send(JsonConvert.SerializeObject(response));
                        }
                        catch (Exception ex)
                        {
                            _ = ws.Send($"Error processing message: {ex.Message}");
                        }
                    }
                };
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}