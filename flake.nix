{
  inputs.nixpkgs.url = "nixpkgs/nixos-24.11";

  outputs = { self, nixpkgs, ... }@inputs:
  let
    pkgs = import nixpkgs { system = "x86_64-linux"; };
    lib = nixpkgs.lib;
  in {
    apps.x86_64-linux.fetch-deps = {
      type = "app";
      program = "${self.packages.x86_64-linux.default.fetch-deps}";
    };
    packages.x86_64-linux.default = pkgs.buildDotnetModule rec {
      name = "AFRA.MEMBERS";
      src = ./.;
      projectFile = "AFRA.Members.sln";
      dotnet-sdk = pkgs.dotnetCorePackages.dotnet_8.sdk;
      dotnet-runtime = pkgs.dotnetCorePackages.dotnet_8.sdk;

      executables = [ "AFRA.Members" ];

      nugetDeps = ./deps.json;
      meta = {
        mainProgram = "AFRA.Members";
      };
    };
    devShells.x86_64-linux.default = pkgs.mkShell {
      packages = [
        pkgs.pkgs.dotnetCorePackages.dotnet_8.sdk
      ];
    };
  };
}
