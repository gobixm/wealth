<script lang="ts">
  import Counter from "./lib/Counter.svelte";
  import TopAppBar, { Row, Section, Title } from "@smui/top-app-bar";
  import IconButton from "@smui/icon-button";
  import { Router, Link, Route } from "svelte-navigator";
  import Settings from "./lib/Settings.svelte";
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
    <Route path="/" primary={false}>Home</Route>
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
