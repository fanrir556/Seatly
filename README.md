# Queuely

## 目錄

1. [專案介紹](#專案介紹)
2. [團隊成員與負責項目](#團隊成員與負責項目)
3. [技術棧](#技術棧)
4. [安裝與運行](#安裝與運行)
5. [功能展示](#功能展示)
6. [未來展望](#未來展望)

## 專案介紹

Queuely是一個提供活動宣傳和報到服務的整合平台。
一般使用者-可以在我們平台搜尋有興趣的活動並報名參加，提前計畫行程，豐富生活更加便利。
活動舉辦方-可以藉由我們平台上傳活動，並提供事先報名及現場簽到的服務，減少現場混亂，提高運營效率。

## 團隊成員與負責項目

### 湯其霖 [<img src="https://avatars.githubusercontent.com/fanrir556" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/fanrir556)

領導小組、統整意見及決策、專案開發時程管理、專案整合、使用者活動登記頁面、主辦方活動簽到頁面、活動資訊頁面、後台管理頁面開發

### 周郁慈 [<img src="https://avatars.githubusercontent.com/yucichou" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/yucichou)

一般會員註冊登入、收藏清單、首頁、搜尋功能、Azure雲端架設

### 邱泓穎 [<img src="https://avatars.githubusercontent.com/tohousanae" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/tohousanae)

廠商會員登入註冊、修改廠商資訊、活動CRUD、圖片上傳功能優化

### 施和融 [<img src="https://avatars.githubusercontent.com/twins0angel" width="50px" style="border-radius: 50%; vertical-align: middle; margin-left: 10px"/>](https://github.com/twins0angel)

點數商城、每日簽到、小遊戲、優惠券、配色更改、管理者登入

## 技術棧

- **前端**：Vue.js, MVC, CSS, JavaScript, jQuery, Bootstrap
- **後端**：MVC, ASP.NET Core
- **資料庫**：Microsoft SQL Server, Entity Framework Core 8
- **雲端平台**：Azure
- **版本控制**：Git

## 安裝與運行

### 後端 (ASP.NET Core Web API)

1. 進入 `Seatly1` 資料夾：

   ```bash
   cd Seatly1
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
