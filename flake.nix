{
  inputs = {
    nixpkgs.url = "nixpkgs/nixos-24.11";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs = { self, flake-utils, nixpkgs, ... }: {
    overlays.default = final: prev: {
      sqlite-interop = final.callPackage (
        { stdenv, fetchurl, unzip, zlib }:

        stdenv.mkDerivation (finalAttrs: {
          pname = "System.Data.Sqlite.Interop";
          version = "1.0.119.0";

          unpackCmd = "mkdir source; unzip $curSrc -d source";
          src = fetchurl {
            url = "https://system.data.sqlite.org/blobs/${finalAttrs.version}/sqlite-netFx-full-source-${finalAttrs.version}.zip";
            hash = "sha256-6olDgXabm2yLqrO2clNZUYa1w/+9qAim3FtQm/St5Qo=";
          };

          buildPhase = ''
            pushd Setup
            bash ./compile-interop-assembly-release.sh
            popd
          '';
          installPhase = ''
            cp -r bin/2013/Release/bin $out
          '';

          buildInputs = [ zlib ];
          nativeBuildInputs = [ unzip ];
        })
      ) {};

      afra-members = final.callPackage (
        { buildDotnetModule, dotnetCorePackages, sqlite-interop }:

        buildDotnetModule {
          name = "AfRA.Members";
          src = self;
          projectFile = "AFRA.Members.sln";
          dotnet-sdk = dotnetCorePackages.dotnet_8.sdk;
          dotnet-runtime = dotnetCorePackages.dotnet_8.sdk;

          executables = [ "AFRA.Members" ];

          postInstall = ''
            rm -rf $out/lib/runtimes
            cp ${sqlite-interop}/* $out/lib/
          '';

          nugetDeps = self + "/deps.json";
          meta = {
            mainProgram = "AFRA.Members";
          };
        }
      ) {};
    };

  } // flake-utils.lib.eachDefaultSystem (system: let
    pkgs = import nixpkgs {
      inherit system;
      overlays = [ self.overlays.default ];
    };
    lib = pkgs.lib;
  in {
    apps.fetch-deps = {
      type = "app";
      program = "${self.packages.${system}.default.fetch-deps}";
    };
    packages.default = pkgs.afra-members;
    devShells.default = pkgs.mkShell {
      packages = [
        pkgs.pkgs.dotnetCorePackages.dotnet_8.sdk
      ];
    };
  });
}
