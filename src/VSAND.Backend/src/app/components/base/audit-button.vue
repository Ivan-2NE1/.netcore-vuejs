<template>
    <button class="btn btn-default"
            v-on:click.stop.prevent="getAuditHistory">
        <span class="far fa-history"></span>
    </button>
</template>

<script>
    export default {
        name: "audit-button",
        props: {
            auditTable: {
                type: String,
                required: true,
            },
            auditId: {
                type: Number,
                required: true,
            }
        },
        data() {
            return {
                table: this.auditTable,
                id: this.auditId,
            };
        },
        watch: {
            auditTable(newval, oldval) {
                this.table = newval;
            },
            auditId(newval, oldval) {
                this.id = newval;
            },
        },
        methods: {
            getAuditHistory() {
                window.open("/audit/?table=" + this.table + "&id=" + this.id, "auditHistory", "height=600,width=500,resizable=yes,scrollbars=yes,toolbars=no,statusbar=no");
            },
        }
    };
</script>