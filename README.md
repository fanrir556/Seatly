# Queuely

## 目錄

1. [專案介紹](#專案介紹)
2. [團隊成員與負責項目](#團隊成員與負責項目)
3. [技術棧](#技術棧)
4. [資料夾結構](#資料夾結構)
5. [安裝與運行](#安裝與運行)
6. [功能展示](#功能展示)
7. [未來展望](#未來展望)

## 專案介紹

Queuely是一個提供活動宣傳和報到服務的整合平台。
一般使用者-可以在我們平台搜尋有興趣的活動並報名參加，提前計畫行程，豐富生活更加便利。
活動舉辦方-可以藉由我們平台上傳活動，並提供事先報名及現場簽到的服務，減少現場混亂，提高運營效率。

## 團隊成員與負責項目

### 施和融 [<img src="https://avatars.githubusercontent.com/twins0angel" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/twins0angel)

領導小組、統整意見及決策、專案開發時程管理、專案整合、資料庫構想、技術支援、導覽列頁尾視覺製作、遊戲電腦組裝系統前後台整體製作、前端整體路由及狀態管理、網站前後端部署

### 王俊婕 [<img src="https://avatars.githubusercontent.com/Gina628" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/Gina628)

整體遊戲資訊抓取、每日定時更新、首頁製作、搜尋系統（關鍵字搜尋）、遊戲資訊系統前台後台視覺製作、技術支援

### 李憶承 [<img src="https://avatars.githubusercontent.com/sr18893828" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/sr18893828)

會員管理系統、遊戲收藏管理系統、會員登入註冊、會員前後台視覺製作

## 技術棧

- **前端**：Vue.js, Vue Router, Pinia, jQuery, Bootstrap
- **後端**：ASP.NET Core Web API
- **資料庫**：Microsoft SQL Server, Entity Framework Core 8
- **爬蟲技術**：C# (HtmlAgilityPack), Python (Request, BeautifulSoup)
- **雲端平台**：Azure
- **版本控制**：Git

## 資料夾結構

本專案初期為一個 ASP.NET Core MVC 項目，中期才全面轉換為 Vue.js、ASP.NET Core Web API 實作前後端分離的開發。

```plaintext
.
├── SteamNexus/            # 初學 ASP.NET Core MVC 的項目
│   ├── Controllers/       # 控制器
│   ├── Models/            # 模型
│   └── Views/             # 視圖
├── steamnexus_client/     # 前端項目 (Vue.js)
│   ├── src/               # 源代碼
│   │   ├── assets/        # 資源文件 (圖片、樣式等)
│   │   ├── components/    # Vue.js 組件
│   │   ├── router/        # 路由設定 (Vue Router)
│   │   ├── store/         # 狀態管理 (Pinia)
│   │   ├── views/         # 視圖
│   │   └── App.vue        # 主應用組件
│   ├── public/            # 靜態資源
│   ├── index.html         # 主 HTML 文件
│   └── package.json       # 項目配置文件
├── SteamNexus_Server/     # 後端項目 (ASP.NET Core Web API)
│   ├── Controllers/       # 控制器
│   ├── Models/            # 模型
|   ├── Dtos/              # 數據傳輸對象 (DTOs)
│   ├── Data/              # 數據庫上下文
│   ├── Migrations/        # 數據庫遷移
│   ├── Services/          # 服務
│   ├── appsettings.json   # 應用程序設置
│   └── Program.cs         # 程序入口
└── README.md              # 說明文件
```

## 安裝與運行

### 前端 (Vue Project)

1. 進入 `steamnexus_client` 資料夾：

   ```bash
   cd steamnexus_client
   ```

2. 安裝套件：

   ```bash
   npm install
   ```

3. 運行開發伺服器：

   ```bash
   npm run dev
   ```

### 後端 (ASP.NET Core Web API)

1. 進入 `SteamNexus_Server` 資料夾：

   ```bash
   cd SteamNexus_Server
   ```

2. 恢復 NuGet 套件：

   ```bash
   dotnet restore
   ```

3. 運行應用：

   ```bash
   dotnet run
   ```

## 功能展示

1. [遊戲資訊系統](#遊戲資訊系統)
2. [遊戲電腦組裝系統](#遊戲電腦組裝系統)
3. [會員系統](#會員系統)

### 遊戲資訊系統

### 遊戲電腦組裝系統

#### 前台

1. 導覽列上方點擊"遊戲電腦組裝"

![遊戲組裝系統](ReadmeFiles/Hardware/HeroBanner.png)

2. 點擊開始可以使用組裝系統(可配合 Filter 過濾)

![過濾系統](ReadmeFiles/Hardware/Filter.gif)

3.可以步驟式的選擇電腦零件，像是你有選 Intel 的 CPU，系統就會幫你篩選出 Intel 的主機板

![電腦零件篩選](ReadmeFiles/Hardware/TenSystem.gif)

4. 一選擇就會加入下方的產品列表，同時幫你計算總瓦數及價格

![產品列表](ReadmeFiles/Hardware/ProductList.png)

5. 當至少有選擇 CPU、GPU、RAM 時，即可啟動遊戲配備需求計算系統，即時去計算當前硬體配置可以滿足多少 Steam 遊戲的最低、建議配備

![配備計算](ReadmeFiles/Hardware/Match.gif)

6. 可以點擊比例下方按鈕進去觀看實際可玩的遊戲列表，由於資料量大，有實作關鍵字搜尋及分頁功能，而點擊遊戲名稱即可前往網站的遊戲資訊系統

![遊戲列表](ReadmeFiles/Hardware/GameList.gif)

7. 如果不想一個一個選擇電腦零件，也有提供現成菜單可供選擇，一樣可以享受完整功能

![切換系統](ReadmeFiles/Hardware/Switch.png)

![菜單系統](ReadmeFiles/Hardware/MenuList.png)

![菜單產品列表](ReadmeFiles/Hardware/MenuProducts.png)

![菜單遊戲需求系統](ReadmeFiles/Hardware/MenuMatch.png)

#### 後台

1. 右上角登入管理員帳號，點擊"後台系統"

![登入管理員](ReadmeFiles/Hardware/AdminLogin.png)

2. 側邊導覽列可以看到硬體系統

![硬體系統](ReadmeFiles/Hardware/AdminSystem.png)

3. 點擊產品管理可以讀取各項分類的產品列表，如果瓦數有務也可以點擊編輯系統即時修正

![產品管理](ReadmeFiles/Hardware/ProductUpdate.gif)

4. 而需要更新時，可以點擊全部零件更新，會呼叫 Web API，啟動 C# 的 HtmlAgilityPack 對電腦零售商網站發送請求及解析，然後利用 LINQ 對資料進行處理後，新產品會寫入資料庫，已有產品則是進行資訊更新

![產品更新1](ReadmeFiles/Hardware/scrape1.png)

![產品更新2](ReadmeFiles/Hardware/scrape2.png)

![產品更新3](ReadmeFiles/Hardware/scrape3.png)

![產品更新4](ReadmeFiles/Hardware/scrape4.png)

![產品更新5](ReadmeFiles/Hardware/scrape5.png)

5. 至於菜單系統，點擊側邊導覽列菜單管理可進入頁面看到前台上架的所有菜單

![菜單管理](ReadmeFiles/Hardware/Menus.png)

6. 新增菜單，點擊"新增菜單"按鈕，跳出新增菜單互動視窗

![新增菜單](ReadmeFiles/Hardware/MenuCreate.png)

![新增菜單 Modal](ReadmeFiles/Hardware/CreateModal.png)

![新增成功](ReadmeFiles/Hardware/CreateSuccess.png)

7. 編輯菜單，點擊菜單上的"編輯"按鈕，跳出編輯菜單互動視窗

![編輯菜單](ReadmeFiles/Hardware/MenuUpdate.png)

![編輯菜單2](ReadmeFiles/Hardware/MenuUpdate2.png)

![編輯菜單3](ReadmeFiles/Hardware/MenuUpdate3.png)

8. 菜單上架，點擊 "上架" checkbox，返回前台即可看到成功上架的菜單；下架同理

![上架菜單](ReadmeFiles/Hardware/onsale.png)

![上架菜單2](ReadmeFiles/Hardware/onsale2.png)

![下架菜單](ReadmeFiles/Hardware/offsale.png)

9. 刪除菜單，此菜單就永遠的消失了~

![刪除菜單](ReadmeFiles/Hardware/MenuDelete.png)

![刪除菜單2](ReadmeFiles/Hardware/MenuDelete2.png)

### 會員系統

## 未來展望

- 優化前後端性能，縮短響應時間
- 後端引用日誌模組，紀錄服務的執行狀況
- 導入容器化技術 Docker、Kubernetes
- 加入自動化測試，提升系統的穩定性和可靠性
