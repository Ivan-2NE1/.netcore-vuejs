<template>
    <div class="panel" v-bind:ref="cRef" v-bind:id="id">
        <div v-bind:class="['panel-hdr', headerClass]">
            <h2 v-bind:ref="'widget-title-' + cRef" v-bind:id="id + 'title'">
                <span v-if="hasIcon" class="pr-1"><i v-bind:class="['far fa fa-fw', widgetIconProper]"></i></span>                
                {{ widgetTitle }}
            </h2>
            <div class="panel-toolbar mr-2" v-if="hasPaging" key="widgetPaging">
                <nav aria-label="Page Results">
                    <ul class="pagination pagination-xs">
                        <li v-bind:class="['page-item', {'disabled': (currentPage - 1) <= 1}]">
                            <button class="page-link" aria-label="First" v-on:click.prevent="$emit('gotopage', 1)" v-bind:disabled="(currentPage - 1) <= 1">
                                <i class="fal fa-chevron-double-left" aria-hidden="true"></i>
                            </button>
                        </li>
                        <li v-bind:class="['page-item', {'disabled': (currentPage - 1) <= 1}]">
                            <button class="page-link" aria-label="Previous" v-on:click.prevent="$emit('gotopage', (currentPage - 1))" v-bind:disabled="(currentPage - 1) <= 1">
                                <i class="fal fa-chevron-left" aria-hidden="true"></i>
                            </button>
                        </li>
                        <li v-for="pg in showPages" v-bind:class="['page-item', {'active': pg === currentPage}]">
                            <button class="page-link" v-bind:aria-label="'Page ' + pg" v-on:click.prevent="$emit('gotopage', pg)" v-bind:disabled="currentPage === pg">{{ pg }}</button>
                        </li>
                        <li v-bind:class="['page-item', {'disabled': (currentPage + 1) > totalPages}]">
                            <button class="page-link" aria-label="Next" v-on:click.prevent="$emit('gotopage', (currentPage + 1))" v-bind:disabled="(currentPage + 1) > totalPages">
                                <i class="fal fa-chevron-right" aria-hidden="true"></i>
                            </button>
                        </li>
                        <li v-bind:class="['page-item', {'disabled': (currentPage + 1) > totalPages}]">
                            <button class="page-link" aria-label="Last" v-on:click.prevent="$emit('gotopage', totalPages)" v-bind:disabled="(currentPage + 1) > totalPages">
                                <i class="fal fa-chevron-double-right" aria-hidden="true"></i>
                            </button>
                        </li>
                    </ul>
                </nav>
            </div>
            <div class="panel-toolbar" v-if="hasToolbarSlot" key="widgetToolbarSlot">
                <slot name="toolbar"></slot>
            </div>
            <div class="panel-toolbar pr-3 align-self-end" v-if="hasToolbarTabsSlot" key="widgetToolbarTabsSlot">
                <slot name="toolbartabs"></slot>
            </div>
            <div class="panel-toolbar" v-if="hasToolbarMasterButtonSlot" key="widgetToolbarMasterButtonSlot">
                <button class="btn btn-toolbar-master" data-toggle="dropdown">
                    <i class="fal fa-ellipsis-v"></i>
                </button>
                <div class="dropdown-menu dropdown-menu-animated dropdown-menu-right">
                    <slot name="toolbarmasterbuttons"></slot>
                </div>
            </div>
        </div>
        <div class="panel-container">
            <div class="panel-content py-2 bg-faded border-faded border-top-0 border-left-0 border-right-0" v-if="hasBodyToolbarSlot" key="widgetBodyToolbar">
                <slot name="bodytoolbar"></slot>
            </div>
            <div v-if="hasBodyMessageSlot" key="widgetBodyMessage" class="panel-content border-faded border-top-0 border-right-0 border-left-0">
                <slot name="bodymessage"></slot>
            </div>
            <div v-bind:class="{'panel-content': widgetPadding}">
                <slot></slot>
            </div>
            <div class="panel-content py-2 rounded-bottom border-faded border-left-0 border-right-0 border-bottom-0 text-muted" v-if="hasFooterSlot" key="widgetFooterSlot">
                <slot name="footer"></slot>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "widget-wrapper",
        props: {
            id: {
                type: String,
                default: () => {
                    return 'widget-wrapper-id-' + Math.random().toString(36).substr(2, 9)
                }
            },
            cRef: {
                type: String,
                default: () => {
                    return 'widget-wrapper-' + Math.random().toString(36).substr(2, 9)
                }
            },
            headerClass: {
                type: String,
                default: "bg-fusion-400 bg-fusion-gradient"
            },
            icon: {
                type: String,
                default: "",
            },
            title: {
                type: String,
                default: "Untitled Widget"
            },
            padding: {
                type: Boolean,
                default: true
            },
            pageSize: {
                type: Number,
                default: null,
            },
            currentPage: {
                type: Number,
                default: null,
            },
            totalRecords: {
                type: Number,
                default: null,
            }
        },

        data() {
            return {
                widgetIcon: this.icon,
                widgetTitle: this.title,
                widgetPadding: this.padding,
            }
        },

        computed: {
            widgetIconProper() {
                var sRet = "";
                if (this.widgetIcon !== null && this.widgetIcon !== "") {
                    sRet = this.widgetIcon;
                    if (this.widgetIcon.substr(3) !== "fa-") {
                        sRet = "fa-" + this.widgetIcon;
                    }
                }
                return sRet;
            },

            hasPaging() {
                return this.pageSize !== null && this.pageSize > 0 && this.currentPage !== null && this.totalRecords !== null && this.totalRecords > 0 && this.totalPages > 1;
            },

            totalPages() {
                return Math.ceil(this.totalRecords / this.pageSize);
            },

            showPages() {
                var oRet = [];
                if ((this.currentPage - 1) >= 1) {
                    oRet.push(this.currentPage - 1);
                }

                oRet.push(this.currentPage);

                if ((this.currentPage + 1) <= this.totalPages) {
                    oRet.push(this.currentPage + 1);
                }

                if (oRet.length < 3 && this.totalPages > 3) {
                    if (this.currentPage + 2 < this.totalPages) {
                        oRet.push(this.currentPage + 2);
                    }
                }

                return oRet;
            },

            hasIcon() {
                return this.widgetIconProper !== "";
            },

            hasTitleSlot() {
                return !!this.$slots.title;
            },

            hasTitleIconSlot() {
                return !!this.$slots.titleIcon;
            },

            hasFooterSlot() {
                return !!this.$slots.footer;
            },

            hasToolbarSlot() {
                return !!this.$slots.toolbar;
            },

            hasToolbarTabsSlot() {
                return !!this.$slots.toolbartabs;
            },

            hasToolbarMasterButtonSlot() {
                return !!this.$slots.toolbarmasterbuttons;
            },

            hasBodyToolbarSlot() {
                return !!this.$slots.bodytoolbar;
            },

            hasBodyMessageSlot() {
                return !!this.$slots.bodymessage;
            }
        }
    };
</script>

<style lang="scss">
    .panel-toolbar ul.pagination {
        margin-bottom: 0;
    }
</style>