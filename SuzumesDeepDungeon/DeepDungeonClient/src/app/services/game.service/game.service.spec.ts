import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { GameService } from './game.service';

import { Store, StoresEnum } from '../../models/store';
import { HttpErrorResponse } from '@angular/common/http';
import { GameRankDTO } from '../../models/game-ranking';
import { GameStatus } from '../../models/game-status.enum';

describe('GameService', () => {
  let service: GameService;
  let httpMock: HttpTestingController;

  // Создаем полный mock-объект согласно всем DTO
  const mockGame: GameRankDTO = {
    id: 1,
    name: 'The Witcher 3: Wild Hunt',
    rate: 97,
    status: GameStatus.Completed,
    gameTime: 150.5,
    review: 'Одна из лучших RPG всех времен',
    created: '2023-01-15T10:30:00Z',
    updated: new Date('2023-01-20T14:45:00Z'),
    user: {
      username: 'geralt_rivia',
      email: 'geralt@rivia.com',
      isAdmin: false
    },
    image: 'witcher3.jpg',
    youtubeLink: 'https://youtu.be/c0i88t0Kacs',
    metacriticRate: 93,
    released: '2015-05-19',
    rawgId: '3328',
    stores: [
      {
        id: 1,
        storeId: StoresEnum.Steam,
        url: 'https://store.steampowered.com/app/292030',
        rawgId: '3328'
      },
      {
        id: 2,
        storeId: StoresEnum.GoG,
        url: 'https://www.gog.com/game/the_witcher_3_wild_hunt',
        rawgId: '3328'
      }
    ],
    screenshots: {
      steamHeaderUrl: 'https://cdn.akamai.steamstatic.com/steam/apps/292030/header.jpg',
      steamCapsuleUrl: 'https://cdn.akamai.steamstatic.com/steam/apps/292030/capsule_184x69.jpg',
      steam600x900Url: 'https://cdn.akamai.steamstatic.com/steam/apps/292030/library_600x900.jpg',
      rawgBackgroundUrl: 'https://media.rawg.io/media/games/618/618c2031a07bbff6b4f611f10b6bcdbc.jpg'
    },
    achievements: [
      {
        id: 1,
        gameId: 1,
        name: 'Сделано в Велене',
        description: 'Выполните все задания в Велене',
        imageUrl: 'achievement1.jpg',
        completionPercent: '15.2%',
        created: '2023-01-16',
        updated: new Date('2023-01-16')
      }
    ],
    trailers: [
      {
        name: 'Трейлер запуска',
        previewImageUrl: 'trailer_thumb.jpg',
        video480p: 'trailer_480p.mp4',
        videoMaxQuality: 'trailer_4k.mp4'
      }
    ],
    tags: [
      {
        name: 'RPG',
        slug: 'rpg',
        language: 'eng',
        gamesCount: 12345,
        imageBackground: 'rpg_bg.jpg'
      }
    ]
  };

  const mockGames: GameRankDTO[] = [
    {
      id: 1,
      name: 'Cyberpunk 2077',
      rate: 85,
      status: GameStatus.InProgress,
      user: {
        username: 'v_cyberpunk',
        email: 'v@nightcity.com',
        isAdmin: false
      }
    },
    {
      id: 2,
      name: 'Red Dead Redemption 2',
      rate: 96,
      status: GameStatus.Completed,
      user: {
        username: 'arthur_morgan',
        email: 'arthur@vanhorn.com',
        isAdmin: true
      }
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [GameService]
    });
    
    service = TestBed.inject(GameService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('должен быть создан', () => {
    expect(service).toBeTruthy();
  });

  describe('getGames()', () => {
    it('должен возвращать список игр с правильной структурой DTO', () => {
      service.getGames().subscribe(games => {
        expect(games.length).toBe(2);
        expect(games[0].name).toBe('Cyberpunk 2077');
        expect(games[0].status).toBe(GameStatus.InProgress);
        expect(games[1].user?.isAdmin).toBeTrue();
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush(mockGames);
    });

    it('должен корректно обрабатывать параметры фильтрации', () => {
      const params = {
        minRate: 90,
        status: GameStatus.Completed,
        tags: 'RPG,OpenWorld',
        sortBy: 'rate',
        desc: true
      };

      service.getGames(params).subscribe();

      const req = httpMock.expectOne(req => 
        req.params.get('minRate') === '90' &&
        req.params.get('status') === 'Completed' &&
        req.params.get('tags') === 'RPG,OpenWorld' &&
        req.params.get('sortBy') === 'rate' &&
        req.params.get('desc') === 'true'
      );
      
      expect(req.request.method).toBe('GET');
      req.flush([mockGames[1]]);
    });
  });

  describe('addGame()', () => {
    it('должен отправлять POST запрос с полным объектом GameRankDTO', () => {
      service.addGame(mockGame).subscribe(game => {
        expect(game.name).toBe('The Witcher 3: Wild Hunt');
        expect(game.stores?.length).toBe(2);
        expect(game.stores?.[0].storeId).toBe(StoresEnum.Steam);
        expect(game.achievements?.[0].name).toBe('Сделано в Велене');
        expect(game.tags?.[0].name).toBe('RPG');
      });

      const req = httpMock.expectOne('DeepDungeon/');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockGame);
      req.flush(mockGame);
    });

    it('должен обрабатывать минимальный объект GameRankDTO', () => {
      const minimalGame: GameRankDTO = {
        id: 3,
        name: 'New Game',
        status: GameStatus.PlantoPlay
      };

      service.addGame(minimalGame).subscribe(game => {
        expect(game.name).toBe('New Game');
        expect(game.status).toBe(GameStatus.PlantoPlay);
        expect(game.stores).toBeNull();
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush(minimalGame);
    });

    it('должен обрабатывать ошибку при дубликате', () => {
      const errorResponse = new HttpErrorResponse({
        status: 409,
        statusText: 'Conflict',
        error: { message: 'Игра с таким названием уже существует' }
      });

      service.addGame(mockGame).subscribe({
        error: (error) => {
          expect(error.status).toBe(409);
          expect(error.error.message).toContain('уже существует');
        }
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush(errorResponse.error, errorResponse);
    });
  });

  describe('updateGame()', () => {
    it('должен обновлять игру с измененными данными', () => {
      const updatedGame: GameRankDTO = {
        ...mockGame,
        rate: 98,
        status: GameStatus.NetworkGame,
        review: 'Обновленный обзор',
        achievements: [
          ...mockGame.achievements!,
          {
              name: 'Мастер Гвинта',
              id: 2,
              gameId: 1,
              description: 'Победите всех противников в Гвинте',
              imageUrl: 'gwent_master.jpg',
              completionPercent: '5.3%',
              created: '',
              updated: ''
          }
        ]
      };

      service.updateGame(updatedGame).subscribe(game => {
        expect(game.rate).toBe(98);
        expect(game.status).toBe(GameStatus.NetworkGame);
        expect(game.achievements?.length).toBe(2);
      });

      const req = httpMock.expectOne('DeepDungeon/');
      expect(req.request.method).toBe('PATCH');
      expect(req.request.body).toEqual(updatedGame);
      req.flush(updatedGame);
    });

    it('должен обрабатывать обновление с удалением вложенных объектов', () => {
      const updatedGame: GameRankDTO = {
        id: 1,
        status: GameStatus.Drop,
        achievements: null,
        trailers: []
      };

      service.updateGame(updatedGame).subscribe(game => {
        expect(game.status).toBe(GameStatus.Drop);
        expect(game.achievements).toBeNull();
        expect(game.trailers).toEqual([]);
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush({...mockGame, ...updatedGame});
    });
  });

  describe('deleteGame()', () => {
    it('должен отправлять DELETE запрос для существующей игры', () => {
      service.deleteGame(1).subscribe(response => {
        expect(response).toBeNull();
      });

      const req = httpMock.expectOne('DeepDungeon/1');
      expect(req.request.method).toBe('DELETE');
      req.flush(null);
    });

    it('должен обрабатывать ошибку при удалении несуществующей игры', () => {
      const errorResponse = new HttpErrorResponse({
        status: 404,
        statusText: 'Not Found',
        error: { message: 'Игра с ID 999 не найдена' }
      });

      service.deleteGame(999).subscribe({
        error: (error) => {
          expect(error.status).toBe(404);
          expect(error.error.message).toContain('не найдена');
        }
      });

      const req = httpMock.expectOne('DeepDungeon/999');
      req.flush(errorResponse.error, errorResponse);
    });
  });

  describe('Обработка сложных структур', () => {
    it('должен корректно работать с вложенными массивами', () => {
      const gameWithArrays: GameRankDTO = {
        id: 10,
        name: 'Game with arrays',
        status: GameStatus.Completed,
        stores: [
          {
            storeId: StoresEnum.Xbox,
            url: 'https://xbox.com/game'
          }
        ],
        achievements: [
          {
              name: 'First Achievement',
              description: 'Complete first level',
              imageUrl: 'achievement1.jpg',
              completionPercent: '45%',
              id: 0,
              gameId: 0,
              created: '',
              updated: ''
          }
        ],
        tags: [
          {
            name: 'Adventure',
            slug: 'adventure'
          }
        ]
      };

      service.addGame(gameWithArrays).subscribe(game => {
        expect(game.stores?.[0].storeId).toBe(StoresEnum.Xbox);
        expect(game.achievements?.[0].completionPercent).toBe('45%');
        expect(game.tags?.[0].slug).toBe('adventure');
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush(gameWithArrays);
    });

    it('должен обрабатывать различные форматы дат', () => {
      const gameWithDates: GameRankDTO = {
        id: 11,
        status: GameStatus.Unknown,
        created: '2024-01-01',
        updated: new Date('2024-01-02T12:00:00Z'),
        achievements: [
          {
              name: 'Time Achievement',
              created: new Date('2024-01-03'),
              updated: '2024-01-04T08:30:00Z',
              id: 0,
              gameId: 0,
              description: '',
              imageUrl: '',
              completionPercent: ''
          }
        ]
      };

      service.addGame(gameWithDates).subscribe(game => {
        expect(typeof game.created).toBe('string');
        expect(game.updated instanceof Date).toBeTruthy();
        expect(game.achievements?.[0].created instanceof Date).toBeTruthy();
        expect(typeof game.achievements?.[0].updated).toBe('string');
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush(gameWithDates);
    });
  });

  describe('Проверка enum', () => {
    it('должен корректно обрабатывать все значения GameStatus', () => {
      const statuses = Object.values(GameStatus);
      
      statuses.forEach(status => {
        const game: GameRankDTO = {
          id: 100 + statuses.indexOf(status),
          name: `Game ${status}`,
          status: status
        };
        
        service.addGame(game).subscribe(g => {
          expect(g.status).toBe(status);
        });

        const req = httpMock.expectOne('DeepDungeon/');
        req.flush(game);
      });
    });

    it('должен корректно обрабатывать все значения StoresEnum', () => {
      const stores: Store[] = [
        { storeId: StoresEnum.Steam },
        { storeId: StoresEnum.GoG },
        { storeId: StoresEnum.Nintendo },
        { storeId: StoresEnum.Microsoft },
        { storeId: StoresEnum.Playstation },
        { storeId: StoresEnum.Xbox },
        { storeId: StoresEnum.Epic },
        { storeId: StoresEnum.Apple },
        { storeId: StoresEnum.Google },
        { storeId: StoresEnum.Itch }
      ];

      const game: GameRankDTO = {
        id: 20,
        name: 'Multi-store Game',
        status: GameStatus.Completed,
        stores: stores
      };

      service.addGame(game).subscribe(g => {
        expect(g.stores?.length).toBe(10);
        expect(g.stores?.map(s => s.storeId)).toEqual([
          StoresEnum.Steam,
          StoresEnum.GoG,
          StoresEnum.Nintendo,
          StoresEnum.Microsoft,
          StoresEnum.Playstation,
          StoresEnum.Xbox,
          StoresEnum.Epic,
          StoresEnum.Apple,
          StoresEnum.Google,
          StoresEnum.Itch
        ]);
      });

      const req = httpMock.expectOne('DeepDungeon/');
      req.flush(game);
    });
  });
});