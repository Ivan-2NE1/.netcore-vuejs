<template>
    <tr>
        <td v-for="(col,idx) in gridCols" v-bind:key="rowId + 'col' + idx">
            <div class="form-inline">
                <input-field v-if="editMode && !readOnlyCols.includes(col.columnName)"
                             v-bind:input-id="rowId + 'colInput' + idx"
                             v-bind:default-value="(gridData[col.columnName] || '').toString()"
                             v-bind:required="col.required"
                             v-bind:max-length="col.maxLength"
                             v-on:input="gridData[col.columnName] = $event"></input-field>
            </div>
            <span v-if="!editMode || readOnlyCols.includes(col.columnName)">{{ gridData[col.columnName] }}</span>
        </td>

        <template v-if="enableEdit || enableDelete || enableExtraActions">
            <td v-if="!editMode">
                <a class="btn btn-primary btn-sm" v-on:click.prevent="EditRow" v-if="enableEdit">Edit</a>
                <a class="btn btn-danger btn-sm" v-on:click.prevent="ConfirmDelete" v-if="enableDelete">Delete</a>
                <slot name="extra-actions" v-bind:row="gridData"></slot>
            </td>
            <td v-if="editMode">
                <a class="btn btn-primary btn-sm" v-on:click.prevent="SaveRow">Save</a>
                <a class="btn btn-default btn-sm" v-on:click.prevent="CancelEdit(true)">Cancel</a>
            </td>
        </template>

        <b-modal v-model="showDeleteModal" centered title="Confirm Delete" v-if="enableDelete">
            <p>Are you sure you want to delete {{ gridData[titleKey] }}?</p>
            <div slot="modal-footer">
                <a class="btn btn-default" v-on:click.prevent="showDeleteModal = false">Cancel</a>
                <a class="btn btn-danger" v-on:click.prevent="DeleteRow">Delete</a>
            </div>
        </b-modal>
    </tr>
</template>

<script>
    import InputField from "../input-field/input-field.vue";
    import { ModalPlugin, ToastPlugin } from "bootstrap-vue";

    Vue.use(ModalPlugin);
    Vue.use(ToastPlugin);

    export default {
        name: "table-grid-row",
        components: {
            "input-field": InputField,
        },
        model: {
            // By default, `v-model` reacts to the `input`
            // event for updating the value, we change this
            // to `change` for similar behavior as the
            // native `<select>` element.
            event: 'change',
        },
        props: {
            rowId: {
                type: String,
                default: "",
            },
            colInfo: {
                // Array of keys from data object in order to display
                type: Array,
                default: null,
            },
            rowData: {
                type: Object,
                default: null
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
                gridOrigData: JSON.parse(JSON.stringify(this.rowData)),
                gridData: JSON.parse(JSON.stringify(this.rowData)),
                editMode: false,
                showDeleteModal: false
            };
        },
        computed: {
            titleKey() {
                if (this.titleProperty === null || this.titleProperty === "") {
                    return this.gridCols[0].columnName
                }

                return this.titleProperty;
            }
        },
        watch: {
            rowColumns(newval) {
                this.gridCols = newval;
            },

            rowData: {
                handler(newval) {
                    this.gridData = JSON.parse(JSON.stringify(newval));
                    this.gridOrigData = JSON.parse(JSON.stringify(newval));

                    // when rowData is updated by the parent app, set edit mode to false
                    this.editMode = false;
                }
            }
        },
        methods: {
            EditRow() {
                this.editMode = true;
                this.$emit("edit-start", this.gridData);
            },
            CancelEdit(showNotificaiton) {
                this.gridData = JSON.parse(JSON.stringify(this.gridOrigData));
                this.editMode = false;

                if (showNotificaiton === true) {
                    this.$bvToast.toast('You have stopped editing ' + this.gridOrigData[this.titleKey] + '. No changes will be saved.', {
                        title: 'Canceled Editing',
                        autoHideDelay: 5000,
                        appendToast: true,
                        solid: true,
                        variant: "warning"
                    });
                }
            },
            SaveRow() {
                this.$emit("change", this.gridData);
            },
            ConfirmDelete() {
                this.showDeleteModal = true;
            },
            DeleteRow() {
                this.$emit("delete", this.gridData);
                this.showDeleteModal = false;
            }
        }
    };
</script>