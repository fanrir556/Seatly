<style></style>

<template>
  <div>
    <button class="btn btn-gradient mb-2 btn-cate" data-cate="all">全部</button>

    <!-- @foreach (var cate in Model.Select(s => s.Category).Distinct())
    {
        <button class="btn btn-gradient mb-2 btn-cate" data-cate="@cate">@cate</button>
    } -->
  </div>

  <div class="d-flex align-items-center">
    <label for="pdSearch" class="text-gradient form-label m-0">關鍵字:</label
    ><input id="pdSearch" type="text" class="form-control ms-2 me-2 w-25" autocomplete="off" />
    <button class="btn btn-gradient me-1" id="btnPdSearch">搜尋</button>
    <button class="btn btn-gradient" id="btnCcSearch">清除搜尋</button>
  </div>
  <div class="d-flex justify-content-end align-items-center">
    <span class="me-2"
      >每頁顯示<select id="pgSize">
        <option value="10" selected>10</option>
        <option value="25">25</option>
        <option value="50">50</option>
        <option value="100">100</option></select
      >筆資料。</span
    >
    <button
      class="btn btn-gradient me-1 p-0"
      id="pP"
      style="width: 30px; height: 30px; text-align: center"
    >
      上
    </button>
    第
    <input
      class="me-1 p-0"
      id="pgNum"
      value="1"
      style="width: 30px; height: 30px; text-align: center"
    />
    頁，共<span id="pgTotal"></span>頁
    <button
      class="btn btn-gradient me-1 p-0"
      id="nP"
      style="width: 30px; height: 30px; text-align: center"
    >
      下
    </button>
  </div>

  <div class="col-12" id="pointsContentBody">
    <!-- if (isMg == "true")
{ -->
    <div class="d-flex justify-content-between mt-1">
      <button class="btn btn-gradient mb-2 me-1" id="btnCreatePS">新增商品</button>
      <button class="btn btn-gradient mb-2 me-1" id="btnSaveEditPS">儲存修改</button>
      <button class="btn btn-gradient mb-2 me-1" id="btnSaveNewPS" style="display: none">
        儲存新增
      </button>
      <button class="btn btn-gradient mb-2 ms-auto" id="btnDeleteSelected">刪除勾選商品</button>
    </div>
    <div id="requiredContent"></div>
    <table class="table table-striped text-center">
      <thead>
        <tr>
          <th style="width: 100px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductId)
          </th>
          <th style="width: 200px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductImage)
          </th>
          <th style="width: 120px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductName)
          </th>
          <th style="width: 80px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductPrice)
          </th>
          <th style="width: 120px">
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().Category)
          </th>
          <th>
            @Html.DisplayNameFor(model => model.pointsShopPd.FirstOrDefault().ProductDescription)
          </th>
          <th style="width: 50px">刪除</th>
        </tr>
      </thead>
      <tbody>
        <tr class="align-middle" id="createPS" style="display: none">
          <td></td>
          <td>
            <img
              :src="noImgUrl"
              class="img-fluid rounded"
              style="width: 100px"
              alt="..."
              title=".."
            />
            <input class="form-control p-0 pimg" type="file" accept="image/*" />
          </td>
          <td><input class="form-control p-0 text-center" style="width: 120px" type="text" /></td>
          <td><input class="form-control p-0 text-center" style="width: 80px" type="number" /></td>
          <td>
            <select class="form-control p-0 text-center" asp-items="ViewBag.PSCategory">
              <option disabled selected>請選擇</option>
            </select>
            <input
              class="form-control p-0 text-center"
              style="width: 104px; display: none"
              type="text"
              placeholder="請輸入類別"
            />
            <p class="text-gradient" id="addCategory">新增類別?</p>
          </td>
          <td><textarea class="form-control p-0"></textarea></td>
          <td></td>
        </tr>
        <!-- @foreach (var item in Model.pointsShopPd)
            { -->
        <tr class="align-middle">
          <td>@item.ProductId</td>
          <td>
            <img
              src="~/images/@item.ProductImage"
              class="img-fluid rounded"
              style="width: 100px"
              alt="@item.ProductImage"
              title="@item.ProductName"
              data-fileName="@item.ProductImage"
            />
            <input class="form-control p-0 pimg" type="file" accept="image/*" />
          </td>
          <td>
            <input
              class="form-control p-0 text-center"
              style="width: 120px"
              type="text"
              value="@item.ProductName"
            />
          </td>
          <td>
            <input
              class="form-control p-0 text-center"
              style="width: 80px"
              type="number"
              value="@item.ProductPrice"
            />
          </td>
          <td>
            <select
              class="form-control p-0 text-center"
              asp-items="ViewBag.PSCategory"
              asp-for="@item.Category"
            ></select>
          </td>
          <td><textarea class="form-control p-0">@item.ProductDescription</textarea></td>
          <td><input type="checkbox" class="form-check-input" /></td>
        </tr>
        <!-- } -->
      </tbody>
    </table>
    <!-- }
else
{
    @if (SignInManager.IsSignedIn(User))
    { -->
    <div>
      <div class="text-gradient d-inline">您目前持有:</div>
      <!-- @if (Model.user.Points == null)
            { -->
      <div class="d-inline ms-2 me-2" id="userPoints">0</div>
      <!-- }
            else
            { -->
      <div class="d-inline ms-2 me-2" id="userPoints" data-userId="@Model.user.Id">
        @Model.user.Points
      </div>
      <!-- } -->
      <div class="text-gradient d-inline">點數</div>
    </div>
    <!-- }
    @foreach (var product in Model.pointsShopPd)
    { -->
    <div class="card mb-3">
      <div class="row g-0">
        <div class="col-md-4">
          <img
            src="~/images/@product.ProductImage"
            class="img-fluid rounded-start"
            style="width: 300px"
            alt="@product.ProductName"
            title="@product.ProductName"
          />
        </div>
        <div class="col-md-8">
          <div class="card-body">
            <span class="position-absolute end-0 me-2">@product.Category</span>
            <h5 class="card-title text-gradient fs-3">@product.ProductName</h5>
            <p class="card-text">@product.ProductDescription</p>
            <p class="card-text text-gradient">兌換需要 @product.ProductPrice 點數</p>
            <button
              class="btn btn-gradient position-absolute bottom-0 end-0 mb-1 me-1 btn-exchange"
              data-bs-toggle="modal"
              data-bs-target="#exchangeModal"
              data-pdId="@product.ProductId"
            >
              兌換
            </button>
          </div>
        </div>
      </div>
    </div>
    <!-- }
} -->
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="exchangeModal"
    tabindex="-1"
    aria-labelledby="exchangeModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exchangeModalLabel">您確定要兌換嗎?</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body" id="exchangeModalBody"></div>
        <div class="modal-footer">
          <button
            class="btn btn-gradient p-0 text-align-center"
            style="width: 100px; height: 45px"
            id="pShopExchange"
          >
            確認兌換
          </button>
          <button
            class="btn btn-secondary p-0 text-align-center"
            style="width: 100px; height: 45px"
            data-bs-dismiss="modal"
          >
            取消
          </button>
        </div>
      </div>
    </div>
  </div>
  <!-- Modal -->
</template>

<script setup>
import { ref, onMounted, reactive } from 'vue'

var noImgUrl = ref(import.meta.env.VITE_API_SeatlyUrl + '/images/NoImage.jpg')
var searchTerm = reactive({
  cate: null,
  pgNum: 1,
  pgSize: 1,
  sortBy: null,
  sortType: 'asc',
  keyword: null
})
</script>
