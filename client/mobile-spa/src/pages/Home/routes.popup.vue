<template>
  <f7-popup class="router-popup"
            @popup:closed="popupOpened = false">
    <f7-page>
      <f7-navbar title="Searchbar">
        <f7-nav-right>
          <f7-link popup-close>Закрыть</f7-link>
        </f7-nav-right>
        <f7-subnavbar :inner="false">
          <f7-searchbar search-container=".search-list"
                        search-in=".item-title"></f7-searchbar>
        </f7-subnavbar>
      </f7-navbar>
      <f7-block>
        <f7-list class="searchbar-not-found">
          <f7-list-item title="Ничего не найдено"></f7-list-item>
        </f7-list>
        <f7-list class="search-list searchbar-found">

        </f7-list>
      </f7-block>
    </f7-page>
  </f7-popup>
</template>
