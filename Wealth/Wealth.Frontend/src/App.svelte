<script lang="ts">
  import TopAppBar, { Row, Section, Title } from "@smui/top-app-bar";
  import IconButton from "@smui/icon-button";
  import { Router, Link, Route } from "svelte-navigator";
  import Settings from "./lib/Settings.svelte";
  import TabBar from "@smui/tab-bar";
  import Tab, { Label } from "@smui/tab";
  import AddTransaction from "./lib/AddTransaction.svelte";

  let activeTab = "Buy/Sell";
</script>

<main>
  <Router>
    <TopAppBar variant="static">
      <Row>
        <Section>
          <Link to="/"><Title>Wealth</Title></Link>
        </Section>
        <Section align="end" toolbar>
          <Link to="settings"
            ><IconButton class="material-icons" aria-label="Settings"
              >build</IconButton
            ></Link
          >
        </Section>
      </Row>
    </TopAppBar>
    <Route path="/" primary={false}>
      <TabBar tabs={["Buy/Sell"]} let:tab bind:active={activeTab}>
        <Tab {tab}>
          <Label>{tab}</Label>
        </Tab>
      </TabBar>

      {#if activeTab === "Buy/Sell"}<AddTransaction />{/if}
    </Route>
    <Route path="settings" primary={false}>
      <Settings />
    </Route>
  </Router>
</main>

<style lang="scss">
  @import "./global.scss";
  :global {
    a {
      all: unset;
    }
  }
  :root {
    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen,
      Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
  }
</style>
