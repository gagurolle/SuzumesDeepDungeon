# 🎮 Streamer Hub - Personal Streaming Portal

This is my passion project: a web application designed to support my streaming channel. It serves as a central hub for my community, featuring a personal bio, a curated game library, Minecraft server updates, and integrated viewer interactions.

## 🚀 Tech Stack

**Frontend:** Angular 18.2  
**Backend:** ASP.NET Core  
**Database:** PostgreSQL with Entity Framework  
**Infrastructure:** 
- Nginx (reverse proxy)
- Docker (containerization)
- GitHub Actions (CI/CD)
- Cloudflare (SSL/TLS termination)
- Domain: [https://deepdungeon.fun](https://deepdungeon.fun) ($2 registration on [https://unstoppabledomains.com](https://unstoppabledomains.com))

## ✨ Features

### 1. 📺 Homepage
- Personal introduction and bio
- Embedded Twitch live stream player
- Direct links to all social media profiles

### 2. 🎯 Top Games Library
**Admin Features:**
- ✅ Add new games to the catalog
- ✏️ Modify existing game entries
- 🗑️ Remove games from the list

**Technical Integration:**
- 🔗 RAWG.io API integration for automatic game data fetching
- 🖼️ Steam cover art retrieval
- 📊 Detailed game pages with genres, tags, and achievements

**User Experience:**
- ♾️ Infinite scroll (lazy loading) for optimal performance
- 🔍 Full-text search capabilities
- ⚙️ Advanced filtering and sorting options
- 🎛️ Contextual menu interface

### 3. 💝 Donation Platforms
- External donation platform links
- *Planned Development:* Cryptocurrency payment service with on-stream alert integration

### 4. 🧱 Minecraft Server Blog
- Community Minecraft server hosted in Docker container
- Static server information card with access details
- Dynamic news feed featuring:
  - 📝 Update announcements
  - 🔧 Mod installations
  - 🆕 Version changes
  - 📢 Important community news

### 5. 🤖 Viewer-Suggested Games (Streamer.Bot Integration)
**Automated Workflow:**
1. **Viewer Action:** Viewer redeems channel points reward with game suggestion
2. **Event Detection:** Streamer.Bot detects redemption event
3. **Secure Transmission:** Data transmitted to secure API endpoint with service key authentication
4. **Server Processing:** Validation and storage of suggestion in database
5. **Frontend Display:** Organized display showing:
   - 👤 Viewer information
   - 🎮 Suggested game title
   - ⭐ Channel points spent
