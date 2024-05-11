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
      >每頁顯示<select v-model="searchTerm.pgSize" @change="showProducts()">
        <option value="10">10</option>
        <option value="25">25</option>
        <option value="50">50</option>
        <option value="100">100</option></select
      >筆資料。</span
    >
    <button
      class="btn btn-gradient me-1 p-0"
      @click="pP"
      style="width: 30px; height: 30px; text-align: center"
      :disabled="firstPage"
    >
      上
    </button>
    第
    <input
      class="me-1 p-0"
      v-model="searchTerm.pgNum"
      @change="showProducts()"
      style="width: 30px; height: 30px; text-align: center"
      min="1"
    />
    頁，共<span>{{ results.totalPg }}</span
    >頁
    <button
      class="btn btn-gradient me-1 p-0"
      @click="nP"
      style="width: 30px; height: 30px; text-align: center"
      :disabled="lastPage"
    >
      下
    </button>
  </div>

  <div class="col-12" id="pointsContentBody">
    <products :isMg="isMg" :results="results"></products>
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
import { ref, onMounted, reactive, watch } from 'vue'
import products from './products.vue'

var firstPage = ref(true)
var lastPage = ref(false)
var isMg = ref(false)
const searchTerm = reactive({
  cate: null,
  pgNum: 1,
  pgSize: 10,
  sortBy: null,
  sortType: 'asc',
  keyword: null
})
const results = reactive({ totalPg: 0, shops: [], userPoints: null, sList: [], dNames: [] })

const showProducts = async () => {
  const APIUrl = import.meta.env.VITE_API_SeatlyUrl + '/api/PointsAPI/pointsShop'
  // const MVCUrl = import.meta.env.VITE_API_SeatlyUrl + '/pointsShop'
  const response = await fetch(APIUrl, {
    method: 'POST',
    body: JSON.stringify(searchTerm),
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json'
    }
  })
  const datas = await response.json()
  console.log(datas)
  results.totalPg = datas.totalPages
  results.shops = datas.shops
  results.userPoints = datas.userPoints
  results.sList = datas.sList
  results.dNames = datas.dNames
  results.shops.forEach((shop) => {
    shop.productImage = import.meta.env.VITE_API_SeatlyUrl + '/images/' + shop.productImage
  })
  isMg.value = datas.isMg
  console.log(results)

  if (results.totalPg <= 1) {
    firstPage.value = true
    lastPage.value = true
  } else {
    lastPage.value = false
  }
}

const pP = () => {
  if (searchTerm.pgNum > 1) {
    searchTerm.pgNum--
    showProducts()
  }
}

const nP = () => {
  if (searchTerm.pgNum < results.totalPg) {
    searchTerm.pgNum++
    showProducts()
  }
}

onMounted(() => {
  showProducts()
})

watch(searchTerm, () => {
  if (isNaN(searchTerm.pgNum) || searchTerm.pgNum < 1) {
    searchTerm.pgNum = 1
  }
  if (searchTerm.pgNum > results.totalPg) {
    searchTerm.pgNum = results.totalPg
  }
  if (searchTerm.pgNum === 1) {
    firstPage.value = true
  } else {
    firstPage.value = false
  }
  if (searchTerm.pgNum === results.totalPg) {
    lastPage.value = true
  } else {
    lastPage.value = false
  }
})
</script>
