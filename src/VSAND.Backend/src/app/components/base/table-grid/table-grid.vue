<template>
    <div class="table-responsive">
        <table class="table">
            <caption v-if="caption !== null && caption !== ''">{{ caption }}</caption>
            <thead>
                <tr>
                    <th v-for="col in colInfo">
                        {{ col.displayName }}
                    </th>
                    <th v-if="enableEdit || enableDelete || enableExtraActions">
                        &nbsp;
                    </th>
                </tr>
            </thead>
            <tbody>
                <table-grid-row v-for="row in gridData"
                                v-bind:ref="'table-grid-row' + row[dataKeyName]"
                                v-bind:key="'table-grid-row' + row[dataKeyName]"
                                v-bind:row-id="'table-grid-row' + row[dataKeyName]"
                                v-bind:row-data="row"
                                v-bind:col-info="gridCols"
                                v-bind:title-property="titleProperty"
                                v-bind:enable-edit="enableEdit"
                                v-bind:enable-delete="enableDelete"
                                v-bind:enable-extra-actions="enableExtraActions"
                                v-on:change="OnChange"
                                v-on:delete="OnDelete"
                                v-on:edit-start="OnEditStart"
                                v-bind:read-only-cols="readOnlyCols">
                    <template v-slot:extra-actions="slotProps">
                        <slot name="extra-actions" v-bind:row="slotProps.row"></slot>
                    </template>
                </table-grid-row>

                <slot name="addformrow"></slot>
            </tbody>
        </table>
    </div>
</template>

<script>
    import TableGridRow from "../table-grid-row/table-grid-row.vue";

    export default {
        name: "table-grid",
        components: {
            "table-grid-row": TableGridRow,
        },
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        props: {
            gridId: {
                type: String,
                required: true,
            },
            colInfo: {
                // Array of keys from data object in order to display
                type: Array,
                default: null,
            },
            allData: {
                type: Array,
                default: null
            },
            caption: {
                type: String,
                default: null,
            },
            dataKeyName: {
                type: String,
                required: true,
            },
            enableEdit: {
                type: Boolean,
                default: false,
            },
            enableDelete: {
                type: Boolean,
                default: false,
            },
            enableExtraActions: {
                type: Boolean,
                default: false,
            },
            titleProperty: {
                type: String,
                default: null
            },
            readOnlyCols: {
                type: Array,
                default: () => []
            }
        },
        data() {
            return {
                gridCols: this.colInfo,
                gridData: this.allData
            };
        },
        watch: {
            colInfo(newval) {
                this.gridCols = newval;
            },

            allData(newval) {
                this.gridData = newval;
            }
        },
        methods: {
            OnChange(e) {
                this.$emit("change", e);
            },
            OnDelete(e) {
                this.$emit("delete", e);
            },
            OnEditStart(e) {
                var targetId = e[this.dataKeyName];
                var rows = this.allData;

                for (let i = 0; i < rows.length; i++) {
                    var row = rows[i];
                    var rowId = row[this.dataKeyName];

                    if (rowId !== targetId) {
                        // cancel editing without showing a notification
                        this.$refs['table-grid-row' + rowId][0].CancelEdit(false);
                    }
                }
            }
        }
    };
</script>